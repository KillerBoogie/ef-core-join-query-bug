using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.GetImages
{
    public interface IGetImagesRepository
    {
        public Task<Result<ImagesResponse>> GetImages(List<Language> languages);
    }
}
