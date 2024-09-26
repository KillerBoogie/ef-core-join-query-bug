using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Range
{
    public class DateTimeRange : ValueObject
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        private DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public static Result<DateTimeRange> Create(DateTime start, DateTime end)
        {
            Result result = Validate(start, end);
            if (result.IsFailure)
                return Result<DateTimeRange>.Failure(result);
            else
                return Result<DateTimeRange>.Success(new(start, end));
        }

        public static Result Validate(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return Result.Failure(Error.Validation.StartDate_Cant_Be_After_EndDate(start, end));
            }
            else
            {
                return Result.Success(); ;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Start.ToString();
            yield return End.ToString();
        }
    }
}
