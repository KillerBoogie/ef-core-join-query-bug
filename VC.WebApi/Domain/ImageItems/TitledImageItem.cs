using VC.WebApi.Domain.Images;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.MultiLanguage;

namespace VC.WebApi.Domain.ImageItems
{
    public record TitledImageItem(
        ImageId ImageId,
        MLRequired<ImageTitle> Title,
        int? DisplayOrder = null,
        Language? Language = null,
        ScreenSize? ScreenSize = null,
        decimal? FocusPointX = null,
        decimal? FocusPointY = null
        )
    {
        // Private parameterless constructor for EF Core
        private TitledImageItem() : this(default!) { }
    }
}
