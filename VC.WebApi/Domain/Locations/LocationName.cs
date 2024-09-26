using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Texts;

namespace VC.WebApi.Domain.Locations
{
    public class LocationName : Text, ICreate<LocationName, string>
    {
        new public static int MinLength => 1;
        new public static int MaxLength => 100;

        private LocationName(string value, int minLength, int maxLength, string fieldName) : base(value, minLength, maxLength, fieldName) { }

        public static Result<LocationName> Create(string text)
        {
            Result result = Validate(text, MinLength, MaxLength, nameof(LocationName));
            if (result.IsFailure)
            {
                return Result<LocationName>.Failure(result);
            }
            else
            {
                return Result<LocationName>.Success(new LocationName(text, MinLength, MaxLength, nameof(LocationName)));
            }
        }
    }
}

