using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Quantities
{
    public class Quantity : ValueObject
    {
        public int Value { get; }

        public bool IsZero => Value == 0;

        public static Quantity Zero => new(0);

        protected Quantity() { }

        protected Quantity(int value)
        {
            Value = value;
        }

        public static Result<Quantity> Create(int value)
        {
            Result result = Validate(value);
            if (result.IsFailure)
                return (Result<Quantity>.Failure(result));
            else
                return Result<Quantity>.Success(new Quantity(value));
        }
        public static Result Validate(int value)
        {
            if (value < 0)
                return Result.Failure(Error.Validation.Must_Be_Pos_Int_Or_Zero(nameof(Quantity), value));

            return Result.Success();
        }

        public static Quantity Of(int value)
        {
            // Allows conversion of const decimal numbers into Quantity
            // In case amount is out of validation range a run-time error is thrown
            return Create(value).Value;
        }

        public virtual Quantity Increase(Quantity quantity)
        {
            return new Quantity(Value + quantity.Value);
        }

        public virtual Quantity Decrease(Quantity quantity)
        {
            return new Quantity(Value - quantity.Value);
        }

        public virtual Quantity Increase(int value)
        {
            return new Quantity(Value + value);
        }

        public virtual Quantity Decrease(int value)
        {
            return new Quantity(Value - value);
        }


        //Quantity-Int
        public static Quantity operator +(Quantity quantity, int value)
        {
            return new Quantity(quantity.Value + value);
        }

        public static Quantity operator -(Quantity quantity, int value)
        {
            return new Quantity(quantity.Value - value);
        }

        public static Quantity operator *(Quantity quantity, int value)
        {
            if (value < 0) throw new ArgumentException("Quantity can't be multiplied by a negative number!", nameof(value));

            return new Quantity(quantity.Value * value);
        }

        public static Quantity operator /(Quantity quantity, int value)
        {
            if (value < 0) throw new ArgumentException("Quantity can't be divided by a negative number!", nameof(value));

            return new Quantity((int)Math.Round((double)quantity.Value / value));
        }

        public static bool operator <(Quantity quantity, int value)
        {
            return quantity.Value < value;
        }
        public static bool operator >(Quantity quantity, int value)
        {
            return quantity.Value > value;
        }

        public static bool operator <=(Quantity quantity, int value)
        {
            return quantity.Value < value;
        }
        public static bool operator >=(Quantity quantity, int value)
        {
            return quantity.Value > value;
        }



        //Quantity-Quantity
        public static Quantity operator +(Quantity quantity1, Quantity quantity2)
        {
            return new Quantity(quantity1.Value + quantity2.Value);
        }
        public static Quantity operator -(Quantity quantity1, Quantity quantity2)
        {
            return new Quantity(quantity1.Value - quantity2.Value);
        }

        public static bool operator <(Quantity quantity1, Quantity quantity2)
        {
            return quantity1.Value < quantity2.Value;
        }
        public static bool operator >(Quantity quantity1, Quantity quantity2)
        {
            return quantity1.Value > quantity2.Value;
        }

        public static bool operator <=(Quantity quantity1, Quantity quantity2)
        {
            return quantity1.Value <= quantity2.Value;
        }
        public static bool operator >=(Quantity quantity1, Quantity quantity2)
        {
            return quantity1.Value >= quantity2.Value;
        }

        public static bool operator ==(Quantity? quantity1, Quantity? quantity2)
        {
            if (quantity1 is null && quantity2 is null)
                return true;

            if (quantity1 is null || quantity2 is null)
                return false;

            return quantity1.Value == quantity2.Value;
        }
        public static bool operator !=(Quantity? quantity1, Quantity? quantity2)
        {
            if (quantity1 is null)
            {
                return !(quantity2 is null);
            }
            else if (quantity2 is null)
            {
                return false;
            }
            else
            {
                return quantity1.Value != quantity2.Value;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
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
