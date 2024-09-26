using MediatR;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.GetImage
{
    public class GetImageQuery(GetImageRequest getImageRequest, EnvironmentDTO environmentDto) : IRequest<Result<ImageResponse>>
    {
        public EnvironmentDTO Environment = environmentDto;
        public GetImageRequest Request = getImageRequest;
    }
}
