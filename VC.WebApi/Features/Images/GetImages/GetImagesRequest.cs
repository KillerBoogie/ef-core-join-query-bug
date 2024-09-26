using VC.WebApi.Shared.Languages;

namespace VC.WebApi.Features.Images.GetImages
{
    public record GetImagesRequest
    (
        List<Language> PreferredLanguages
    );
}
