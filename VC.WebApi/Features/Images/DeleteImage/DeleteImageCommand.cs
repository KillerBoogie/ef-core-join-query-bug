using MediatR;
using VC.WebApi.Infrastructure.MediatR.Pipelines;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.DeleteImage
{
    public class DeleteImageCommand(DeleteImageRequest deleteImageRequest, EnvironmentDTO environmentDto) : IRequest<Result>, ITransactional
    {
        public EnvironmentDTO Environment = environmentDto;
        public DeleteImageRequest DeleteImageRequest = deleteImageRequest;
    }
}
