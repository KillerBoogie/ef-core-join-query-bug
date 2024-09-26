using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Texts;

namespace VC.WebApi.Domain.ImageItems
{
    public class ScreenSize : Text, ICreate<ScreenSize, string>
    {
        new public static int MinLength => 1;
        new public static int MaxLength => 50;

        private ScreenSize(string value, int minLength, int maxLength, string fieldName) : base(value, minLength, maxLength, fieldName) { }

        public static Result<ScreenSize> Create(string text)
        {
            Result result = Validate(text, MinLength, MaxLength, nameof(ScreenSize));
            if (result.IsFailure)
            {
                return Result<ScreenSize>.Failure(result);
            }
            else
            {
                return Result<ScreenSize>.Success(new ScreenSize(text, MinLength, MaxLength, nameof(ScreenSize)));
            }
        }
    }
}

