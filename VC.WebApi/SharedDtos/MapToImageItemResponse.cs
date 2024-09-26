using VC.WebApi.Domain.ImageItems;
using VC.WebApi.Domain.Images;

namespace VC.WebApi.SharedDtos
{
    internal static partial class Mappings
    {
        public static ImageItemResponse MapToImageItemResponse(ImageItem imageItem, Image image)
        {
            return new ImageItemResponse(
                imageItem.ImageId.Value,
                imageItem.DisplayOrder,
                imageItem.Language?.Tag,
                imageItem.ScreenSize?.Value,
                imageItem.FocusPointX?.Value,
                imageItem.FocusPointY?.Value,
                image.Uri.ToString(),
                image.MetaData.Width,
                image.MetaData.Height
            );
        }
    }
}