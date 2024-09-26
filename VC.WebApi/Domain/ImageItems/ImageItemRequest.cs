namespace VC.WebApi.Domain.ImageItems
{
    public record ImageItemRequest(
        Guid ImageId,
        int? DisplayOrder = null,
        string? Language = null,
        string? ScreenSize = null,
        decimal? FocusPointX = null,
        decimal? FocusPointY = null
    );
}
