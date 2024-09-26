using VC.WebApi.Domain.Images;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Domain.ImageItems
{
    static class TitledImageItemRequestExtension
    {
        public static Result<TitledImageItem> CreateTitledImage(this TitledImageItemRequest request)
        {
            ErrorList errors = new();

            MLRequired<ImageTitle>? mlTitle = null;

            if (request.Title.Count == 0)
            {
                errors.Add(Error.Validation.Must_Not_Be_Empty(nameof(request.Title)));
            }
            else
            {
                foreach (KeyValuePair<string, string> item in request.Title)
                {
                    Result<ImageTitle> resultImageTitle = ImageTitle.Create(item.Value);
                    errors.AddIfFailure(resultImageTitle);

                    Result<Language> resultLanguage = Language.Create(item.Key);
                    errors.AddIfFailure(resultLanguage);

                    if (mlTitle is null)
                    {
                        mlTitle = new(resultLanguage.Value, resultImageTitle.Value);
                    }
                    else
                    {
                        mlTitle.Add(resultLanguage.Value, resultImageTitle.Value);
                    }
                }
            }

            if (errors.HasErrors)
            {
                return Result<TitledImageItem>.Failure(errors);
            }

            return Result<TitledImageItem>.Success(
                new(ImageId.CreateFromGuid(request.ImageId), mlTitle!, request.DisplayOrder)
                );
        }
    }
}
