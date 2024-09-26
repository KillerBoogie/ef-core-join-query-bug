using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Texts;

namespace VC.WebApi.Domain.Images
{
    public class ImageDescription : Text, ICreate<ImageDescription, string>
    {
        new public static int MinLength => 1;
        new public static int MaxLength => 4000;
        private ImageDescription(string value, int minLength, int maxLength, string fieldName) : base(value, minLength, maxLength, fieldName) { }

        public static Result<ImageDescription> Create(string text)
        {

            Result result = Validate(text, MinLength, MaxLength, nameof(ImageDescription));
            if (result.IsFailure)
            {
                return Result<ImageDescription>.Failure(result);
            }
            else
            {
                return Result<ImageDescription>.Success(new ImageDescription(text, MinLength, MaxLength, nameof(ImageDescription)));
            }
        }
    }
}
