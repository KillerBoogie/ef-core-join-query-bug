using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.GetImage
{
    public interface IGetImageRepository
    {
        public Task<Result<ImageResponse>> GetImage(GetImageRequest request);
    }
}
