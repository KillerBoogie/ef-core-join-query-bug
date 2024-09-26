using System.Text.RegularExpressions;

namespace VC.WebApi.Shared.EFCore
{
    public class ErrorMessageParser
    {
        public static (string constraintName, string databaseName, string tableName, string columnName) ParseFKMessage(string errorMessage)
        {
            // Implement logic to parse and extract referenced object names from the error message
            string pattern = "\"([^\"]*)\"|'([^']*)'";

            // Create a regex object
            Regex regex = new Regex(pattern);

            // Find matches
            MatchCollection matches = regex.Matches(errorMessage);

            try
            {
                return (matches[0].Groups[1].Value, matches[1].Groups[1].Value, matches[2].Groups[1].Value, matches[3].Groups[2].Value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"The sql server error message of the foreign key violation did not include the expected parameters. ErrorMessage: {errorMessage}", ex);
            }
        }

        public static string MapToResource(string tableName)
        {
            switch (tableName)
            {
                case "VC.EventDance":
                    return "Event";
                case "VC.EventTeacher":
                    return "Event";
                case "VC.EventLocation":
                    return "Event";
                default:
                    return tableName.Substring(3);
            }
        }
    }
}
