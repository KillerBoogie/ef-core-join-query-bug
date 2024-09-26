using MediatR;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.GetImages
{
    public class GetImagesQuery(GetImagesRequest request,
        EnvironmentDTO Environment) : IRequest<Result<ImagesResponse>>
    {
        public GetImagesRequest Request = request;
        public EnvironmentDTO Environment = Environment;
    }
}
