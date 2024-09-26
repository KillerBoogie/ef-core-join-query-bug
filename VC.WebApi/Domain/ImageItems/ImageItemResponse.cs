namespace VC.WebApi.Domain.ImageItems
{
    public record ImageItemResponse(
        Guid Id,
        int? DisplayOrder,
        string? Language,
        string? ScreenSize,
        decimal? FocusPointX,
        decimal? FocusPointY,
        string Uri,
        long? Width,
        long? Height
    );
}
