using MediatR;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.GetImage
{
    public class GetImageHandler(IGetImageRepository GetImageRepository)
        : IRequestHandler<GetImageQuery, Result<ImageResponse>>
    {
        public async Task<Result<ImageResponse>> Handle(GetImageQuery query, CancellationToken cancellationToken)
        {
            EnvironmentDTO environment = query.Environment;
            GetImageRequest request = query.Request;

            return await GetImageRepository.GetImage(request);
        }
    }
}
