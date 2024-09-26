using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Texts;

namespace VC.WebApi.Domain.Countries
{
    public class CountryName : Text, ICreate<CountryName, string>
    {
        // Longest: The United Kingdom of Great Britain and Northern Ireland = 56; min = 4
        new public static int MinLength => 4;
        new public static int MaxLength => 60;
        private CountryName(string value, int minLength, int maxLength, string fieldName) : base(value, minLength, maxLength, fieldName) { }

        public static Result<CountryName> Create(string text)
        {
            Result result = Validate(text, MinLength, MaxLength, nameof(CountryName));
            if (result.IsFailure)
            {
                return Result<CountryName>.Failure(result);
            }
            else
            {
                return Result<CountryName>.Success(new CountryName(text, MinLength, MaxLength, nameof(CountryName)));
            }
        }
    }
}
