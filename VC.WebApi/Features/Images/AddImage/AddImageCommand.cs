using MediatR;
using VC.WebApi.Infrastructure.MediatR.Pipelines;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Responses;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.AddImage
{
    public class AddImageCommand(AddImageRequest addImageRequest, EnvironmentDTO environmentDto) : IRequest<Result<GuidIdResponse>>, ITransactional
    {
        public EnvironmentDTO Environment = environmentDto;
        public AddImageRequest AddImageRequest = addImageRequest;
    }
}
