namespace VC.WebApi.Domain.ImageItems
{
    public record TitledImageItemRequest(
        Guid ImageId,
        Dictionary<string, string> Title,
        int? DisplayOrder = null,
        string? Language = null,
        string? ScreenSize = null,
        decimal? FocusPointX = null,
        decimal? FocusPointY = null
    );
}
