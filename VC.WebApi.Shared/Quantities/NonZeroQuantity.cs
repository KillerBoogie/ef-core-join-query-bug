using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Quantities
{
    public class NonZeroQuantity : Quantity
    {
        protected NonZeroQuantity() : base() { }

        protected NonZeroQuantity(int value) : base(value)
        {
        }

        public static new Result<NonZeroQuantity> Create(int value)
        {
            Result result = Validate(value);
            if (result.IsFailure)
                return (Result<NonZeroQuantity>.Failure(result));
            else
                return Result<NonZeroQuantity>.Success(new NonZeroQuantity(value));
        }
        public static new Result Validate(int value)
        {
            if (value <= 0)
                return Result.Failure(Error.Validation.Must_Be_Pos_Int(nameof(Quantity), value));

            return Result.Success();
        }

        public static new NonZeroQuantity Of(int value)
        {
            // Allows conversion of const decimal numbers into Quantity
            // In case amount is out of validation range a run-time error is thrown
            return Create(value).Value;
        }

        public new NonZeroQuantity Increase(Quantity quantity)
        {
            return new NonZeroQuantity(Value + quantity.Value);
        }

        public new NonZeroQuantity? Decrease(Quantity quantity)
        {
            int decrease = Value - quantity.Value;
            return decrease <= 0 ? null : new NonZeroQuantity(decrease);
        }

        public new NonZeroQuantity Increase(int value)
        {
            return new NonZeroQuantity(Value + value);
        }

        public new NonZeroQuantity? Decrease(int value)
        {
            int decrease = Value - Value;
            return decrease <= 0 ? null : new NonZeroQuantity(decrease);
        }


        public static NonZeroQuantity operator *(NonZeroQuantity quantity, int value)
        {
            if (value < 0) throw new ArgumentException("NonZeroQuantity can't be multiplied by a negative number!", nameof(value));

            return new NonZeroQuantity(quantity.Value * value);
        }

        public static NonZeroQuantity operator /(NonZeroQuantity quantity, int value)
        {
            if (value < 0) throw new ArgumentException("NonZeroQuantity can't be divided by a negative number!", nameof(value));

            int result = (int)Math.Round((double)quantity.Value / value);

            if (result == 0) throw new ArgumentException($"The Division of NonZeroQuantity of '{quantity.Value}' by the integer '{value}' results in 0, which is not valid!", nameof(value));

            return new NonZeroQuantity();
        }

        public static NonZeroQuantity operator *(int value, NonZeroQuantity quantity)
        {
            return value * quantity;
        }

        public static NonZeroQuantity operator /(int value, NonZeroQuantity quantity)
        {
            return value / quantity;
        }

        //NonZeroQuantity-Quantity
        //NonZeroQuantity-NonZeroQuantity is not required, because NonZeroQuantity is a super class of Quantity
        public static Quantity operator +(NonZeroQuantity quantity1, Quantity quantity2)
        {
            return new NonZeroQuantity(quantity1.Value + quantity2.Value);
        }
        public static Quantity operator +(Quantity quantity1, NonZeroQuantity quantity2)
        {
            return quantity1 + quantity2;
        }
        public static Quantity? operator -(NonZeroQuantity quantity1, Quantity quantity2)
        {
            int decrease = quantity1.Value - quantity2.Value;
            return decrease <= 0 ? null : new NonZeroQuantity(decrease);
        }
        public static Quantity? operator -(Quantity quantity1, NonZeroQuantity quantity2)
        {
            return quantity1 - quantity2;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
