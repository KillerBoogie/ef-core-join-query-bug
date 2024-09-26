using VC.WebApi.Domain.Images;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.AddImage
{
    public interface IAddImageRepository
    {
        public Task<Result> AddImage(Image image);
        public Task<Result> IsUriUnique(Uri uri);
    }
}
