using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;


namespace VC.WebApi.Shared.Texts
{
    public abstract class Text : ValueObject, IJson
    {
        public string Value { get; init; }
        public int MinLength { get; init; }
        public int MaxLength { get; init; }
        public string FieldName { get; init; }

        protected Text(string value, int minLength, int maxLength, string fieldName)
        {
            Value = value;
            MinLength = minLength;
            MaxLength = maxLength;
            FieldName = fieldName;
        }


        public static Result Validate(string text, int minLength, int maxLength, string fieldKey)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Result.Failure(Error.Validation.Not_Only_Whitespace(fieldKey));

            if (text.Length > maxLength)
                return Result.Failure(Error.Validation.Max_Char_Length_Exceeded(fieldKey, maxLength));

            if (text.Length < minLength)
                return Result.Failure(Error.Validation.Min_Char_Length_Deceeded(fieldKey, minLength));

            return Result.Success();
        }

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
