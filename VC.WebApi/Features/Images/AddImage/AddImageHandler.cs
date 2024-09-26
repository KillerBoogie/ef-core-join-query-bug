using MediatR;
using VC.WebApi.Domain.Images;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Auditing;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Files;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Responses;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.AddImage
{
    public class AddImageHandler(IAddImageRepository AddImageRepository)
        : IRequestHandler<AddImageCommand, Result<GuidIdResponse>>
    {
        public async Task<Result<GuidIdResponse>> Handle(AddImageCommand command, CancellationToken cancellationToken)
        {
            EnvironmentDTO environment = command.Environment;
            AddImageRequest request = command.AddImageRequest;

            Result<Image> imageResult = CreateImageFromDto(request, environment, DateTime.UtcNow);
            if (imageResult.IsFailure)
            {
                return Result<GuidIdResponse>.Failure(imageResult);
            }
            Image image = imageResult.Value;

            Result isUriUniqueResult = await AddImageRepository.IsUriUnique(image.Uri);
            if (isUriUniqueResult.IsFailure)
            {
                return Result<GuidIdResponse>.Failure(isUriUniqueResult);
            }

            Result result = await AddImageRepository.AddImage(image);
            if (result.IsFailure)
            {
                return Result<GuidIdResponse>.Failure(result);
            }

            return Result<GuidIdResponse>.Success(new GuidIdResponse(image.Id.Value));
        }

        private static Result<Image> CreateImageFromDto(AddImageRequest request, EnvironmentDTO environment, DateTime utcNow)
        {
            ErrorList errors = new();

            Result<FileName> imageNameResult = FileName.Create(request.FileName);
            errors.AddIfFailure(imageNameResult);

            ML<ImageDescription> mlDescription = new();
            if (request.Description != null)
            {
                Result<ML<ImageDescription>> descriptionResult = ML<ImageDescription>.Create<ImageDescription>(request.Description);
                errors.AddIfFailure(descriptionResult);
                mlDescription = descriptionResult.Value;
            }

            bool uriResult = Uri.TryCreate(request.Uri, UriKind.RelativeOrAbsolute, out Uri? imageUri);
            if (!uriResult)
            {
                errors.Add(Error.Validation.Invalid_Uri(nameof(request.Uri)));
            }

            Result<CreatedInfo> createdInfoResult = CreatedInfo.Create(utcNow, environment.Actor, environment.ActingInNameOf); //refactor: getDateTime from parameter
            errors.AddIfFailure(createdInfoResult);

            Result<ImageMetaData> imageMetaDataResult = ImageMetaData.Create(request.Size, request.Width, request.Height);
            errors.AddIfFailure(imageMetaDataResult);

            if (errors.HasErrors)
                return Result<Image>.Failure(errors);

            ImageId imageId = ImageId.Create();
            Result<Image> imageResult = Image.Create(imageId, imageUri!, imageNameResult.Value, mlDescription, imageMetaDataResult.Value, createdInfoResult.Value);

            if (imageResult.IsFailure)
            {
                return Result<Image>.Failure(imageResult);
            }
            else
            {
                return Result<Image>.Success(imageResult.Value);
            }
        }
    }
}
