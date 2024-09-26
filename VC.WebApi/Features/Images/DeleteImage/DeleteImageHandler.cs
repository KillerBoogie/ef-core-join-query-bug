using MediatR;
using VC.WebApi.Domain.Images;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.DeleteImage
{
    public class DeleteImageHandler(IDeleteImageRepository DeleteImageRepository)
        : IRequestHandler<DeleteImageCommand, Result>
    {
        public async Task<Result> Handle(DeleteImageCommand command, CancellationToken cancellationToken)
        {
            EnvironmentDTO environment = command.Environment;
            DeleteImageRequest request = command.DeleteImageRequest;

            ImageId imageId = ImageId.CreateFromGuid(request.Id);

            return await DeleteImageRepository.DeleteImage(imageId);
        }
    }
}
