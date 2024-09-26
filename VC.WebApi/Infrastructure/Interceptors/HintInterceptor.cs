using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;

//https://entityframeworkcore.com/knowledge-base/37984312/how-to-implement-select-for-update-in-ef-core
//https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors

public class TaggedQueryCommandInterceptor : DbCommandInterceptor
{
    private static readonly Regex _tableAliasRegex = new Regex(@"(?<tableAlias>FROM (\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(\*HINT\*\)))", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        ManipulateCommand(command);

        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        ManipulateCommand(command);

        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private static void ManipulateCommand(DbCommand command)
    {
        if (command.CommandText.StartsWith("-- ForUpdate", StringComparison.Ordinal))
        {
            command.CommandText = Replace(command.CommandText, "UPDLOCK");
        }
        else if (command.CommandText.StartsWith("-- ForUpdateHold", StringComparison.Ordinal))
        {
            command.CommandText = Replace(command.CommandText, "HOLDLOCK, UPDLOCK");
        }
        else if (command.CommandText.StartsWith("-- ForTableLock", StringComparison.Ordinal))
        {
            command.CommandText = Replace(command.CommandText, "TABLOCK");
        }
    }

    private static string Replace(string input, string hintValue)
    {
        if (!String.IsNullOrWhiteSpace(hintValue))
        {
            if (!_tableAliasRegex.IsMatch(input))
            {
                throw new InvalidProgramException("Could not identify a table to mark for update!", new Exception(input));
            }
            input = _tableAliasRegex.Replace(input, "${tableAlias} WITH (*HINT*)");
            input = input.Replace("*HINT*", hintValue);
        }

        return input;
    }
}