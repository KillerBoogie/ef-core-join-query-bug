using VC.WebApi.Domain.Images;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Domain.ImageItems
{
    static class ImageItemRequestExtension
    {
        public static Result<ImageItem> CreateImageItem(this ImageItemRequest request)
        {
            ErrorList errors = new();

            if (request.DisplayOrder is not null && request.DisplayOrder < 0)
            {
                errors.Add(Error.Validation.Must_Be_Pos_Int(nameof(request.DisplayOrder), (int)request.DisplayOrder));
            }

            Language? language = null;
            if (request.Language is not null)
            {
                Result<Language> languageResult = Language.Create(request.Language);
                errors.AddIfFailure(languageResult);
                if (languageResult.IsSuccess)
                {
                    language = languageResult.Value;
                }
            }

            ScreenSize? screenSize = null;
            if (request.ScreenSize is not null)
            {
                Result<ScreenSize> screenSizeResult = ScreenSize.Create(request.ScreenSize);
                errors.AddIfFailure(screenSizeResult);
                if (screenSizeResult.IsSuccess)
                {
                    screenSize = screenSizeResult.Value;
                }
            }

            FocusPoint? focusPointX = null;
            if (request.FocusPointX is not null)
            {
                Result<FocusPoint> FocusPointResult = FocusPoint.Create((decimal)request.FocusPointX);
                errors.AddIfFailure(FocusPointResult);
                if (FocusPointResult.IsSuccess)
                {
                    focusPointX = FocusPointResult.Value;
                }
            }

            FocusPoint? focusPointY = null;
            if (request.FocusPointY is not null)
            {
                Result<FocusPoint> FocusPointResult = FocusPoint.Create((decimal)request.FocusPointY);
                errors.AddIfFailure(FocusPointResult);
                if (FocusPointResult.IsSuccess)
                {
                    focusPointY = FocusPointResult.Value;
                }
            }

            if (errors.HasErrors)
            {
                return Result<ImageItem>.Failure(errors);
            }

            return Result<ImageItem>.Success(
                new(ImageId.CreateFromGuid(request.ImageId), request.DisplayOrder, language, screenSize, focusPointX, focusPointY)
                );
        }

        public static Result<TitledImageItem> CreateTitledImageItem(this TitledImageItemRequest request)
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

            if (request.DisplayOrder is not null && request.DisplayOrder < 0)
            {
                errors.Add(Error.Validation.Must_Be_Pos_Int(nameof(request.DisplayOrder), (int)request.DisplayOrder));
            }

            Language? language = null;
            if (request.Language is not null)
            {
                Result<Language> languageResult = Language.Create(request.Language);
                errors.AddIfFailure(languageResult);
                if (languageResult.IsSuccess)
                {
                    language = languageResult.Value;
                }
            }

            ScreenSize? screenSize = null;
            if (request.ScreenSize is not null)
            {
                Result<ScreenSize> screenSizeResult = ScreenSize.Create(request.ScreenSize);
                errors.AddIfFailure(screenSizeResult);
                if (screenSizeResult.IsSuccess)
                {
                    screenSize = screenSizeResult.Value;
                }
            }

            if (errors.HasErrors)
            {
                return Result<TitledImageItem>.Failure(errors);
            }

            return Result<TitledImageItem>.Success(
                new(ImageId.CreateFromGuid(request.ImageId), mlTitle!, request.DisplayOrder, language, screenSize)
                );
        }
    }
}
