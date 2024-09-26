using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Domain.ImageItems
{
    public class FocusPoint : ValueObject
    {
        public static int DecimalPlaces => 2;
        public static int IntegerPlaces => 3;
        public static decimal MaxValue => 100.00m;

        public decimal Value { get; private set; }

        private FocusPoint(decimal value)
        {
            Value = value;
        }

        public static Result<FocusPoint> Create(decimal value)
        {
            Result result = Validate(value);
            if (result.IsFailure)
            {
                return Result<FocusPoint>.Failure(result);
            }
            else
            {
                return Result<FocusPoint>.Success(new FocusPoint(value));
            }
        }

        private static Result Validate(decimal value)
        {

            ErrorList errors = new();

            if (value > MaxValue)
            {
                errors.Add(Error.Validation.Max_Value_Exceeded(nameof(FocusPoint), MaxValue));
            }
            if ((value * 100) % 1 != 0)
            {
                errors.Add(Error.Validation.Max_Decimal_Places_Exceeded(nameof(FocusPoint), DecimalPlaces));
            }

            if (errors.HasErrors)
            {
                return Result.Failure(errors);
            }
            else
            {
                return Result.Success();
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

