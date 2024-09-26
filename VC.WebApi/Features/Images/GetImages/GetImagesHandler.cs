using MediatR;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.GetImages
{
    public class GetImagesHandler(IGetImagesRepository getImagesRepository)
        : IRequestHandler<GetImagesQuery, Result<ImagesResponse>>
    {
        public async Task<Result<ImagesResponse>> Handle(GetImagesQuery query, CancellationToken cancellationToken)
        {
            List<Language> langs = query.Request.PreferredLanguages;

            return await getImagesRepository.GetImages(langs);
        }
    }
}
