using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Domain.DisplayOrders
{
    public class DisplayOrder : ValueObject
    {
        public int Value { get; private set; }
        private DisplayOrder(int value)
        {
            Value = value;
        }
        public static Result<DisplayOrder> Create(int value)
        {
            Result result = Validate(value);
            if (result.IsFailure)
                return (Result<DisplayOrder>.Failure(result));
            else
                return Result<DisplayOrder>.Success(new DisplayOrder(value));
        }
        public static Result Validate(int value)
        {
            if (value <= 0)
                return Result.Failure(Error.Validation.Must_Be_Pos_Int(nameof(DisplayOrder), value));

            return Result.Success();
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
