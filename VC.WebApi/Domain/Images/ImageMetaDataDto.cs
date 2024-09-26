namespace VC.WebApi.Domain.Images
{
    public record ImageMetaDataDto(
        long? Size = null,
        int? Width = null,
        int? Height = null,
        decimal? FocusPointX = null,
        decimal? FocusPointY = null
     );
}
