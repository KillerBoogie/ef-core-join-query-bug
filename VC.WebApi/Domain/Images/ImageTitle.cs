using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Texts;

namespace VC.WebApi.Domain.Images
{
    public class ImageTitle : Text, ICreate<ImageTitle, string>
    {
        new public static int MinLength => 1;
        new public static int MaxLength => 125; // equals HtmlAltTag 
        private ImageTitle(string value, int minLength, int maxLength, string fieldName) : base(value, minLength, maxLength, fieldName) { }

        public static Result<ImageTitle> Create(string text)
        {

            Result result = Validate(text, MinLength, MaxLength, nameof(ImageTitle));
            if (result.IsFailure)
            {
                return Result<ImageTitle>.Failure(result);
            }
            else
            {
                return Result<ImageTitle>.Success(new ImageTitle(text, MinLength, MaxLength, nameof(ImageTitle)));
            }
        }
    }
}
