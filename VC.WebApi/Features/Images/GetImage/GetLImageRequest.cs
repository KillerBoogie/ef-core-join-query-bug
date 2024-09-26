using VC.WebApi.Shared.Languages;

namespace VC.WebApi.Features.Images.GetImage
{
    public record GetImageRequest
    (
        Guid ImageId,
        List<Language> PreferredLanguages
    );
}
