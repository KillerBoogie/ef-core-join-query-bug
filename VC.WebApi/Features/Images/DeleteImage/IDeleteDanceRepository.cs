using VC.WebApi.Domain.Images;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.DeleteImage
{
    public interface IDeleteImageRepository
    {
        public Task<Result> DeleteImage(ImageId imageId);
    }
}
