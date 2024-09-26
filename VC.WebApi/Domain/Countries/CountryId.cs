using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Domain.Countries
{
    public class CountryId : ValueObject
    {
        public string Value { get; protected set; }

        public const int LengthCountryId = 3;
#pragma warning disable CS8618
        private CountryId()
#pragma warning restore CS8618
        { }
        private CountryId(string iso3)
        {
            Value = iso3;
        }

        public static Result<CountryId> CreateFromString(string iso3)
        {
            if (iso3.Length != LengthCountryId)
                return Result<CountryId>.Failure(Error.Validation.Char_Length_Incorrect(nameof(CountryId), LengthCountryId));
            else
                return Result<CountryId>.Success(new CountryId(iso3));
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

