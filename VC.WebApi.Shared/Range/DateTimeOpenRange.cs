using System.Text.Json;
using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Range
{
    public class DateTimeOpenRange : ValueObject
    {
        public DateTime? Start { get; } = null;
        public DateTime? End { get; } = null;

        private DateTimeOpenRange() { }

        private DateTimeOpenRange(DateTime? start, DateTime? end)
        {
            Start = start;
            End = end;
        }

        public static Result<DateTimeOpenRange> Create(DateTime? start, DateTime? end)
        {
            Result result = Validate(start, end);
            if (result.IsFailure)
                return Result<DateTimeOpenRange>.Failure(result);
            else
                return Result<DateTimeOpenRange>.Success(new(start, end));
        }

        public static Result Validate(DateTime? start, DateTime? end)
        {
            if (start is not null && end is not null && start > end)
            {
                return Result.Failure(Error.Validation.FromDate_Must_Be_Before_UntilDate((DateTime)start, (DateTime)end));
            }
            else
            {
                // one or both can be null
                return Result.Success(); ;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Start?.ToString() ?? "";
            yield return End?.ToString() ?? "";
        }

        public DateTimeOpenRange DeepClone()
        {
            return new DateTimeOpenRange(Start, End);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
