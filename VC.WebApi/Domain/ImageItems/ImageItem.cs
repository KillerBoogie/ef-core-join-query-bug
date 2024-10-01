using VC.WebApi.Domain.Images;
using VC.WebApi.Shared.Languages;

namespace VC.WebApi.Domain.ImageItems
{
    public record ImageItem
    (
        ImageId ImageId,
        int? DisplayOrder = null,
        Language? Language = null,
        ScreenSize? ScreenSize = null,
        FocusPoint? FocusPointX = null,
        FocusPoint? FocusPointY = null
        );
}
