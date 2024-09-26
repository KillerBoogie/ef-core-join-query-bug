namespace VC.WebApi.Domain.ImageItems
{
    public record TitledImageItemMLResponse
    (
        Guid Id,
        Dictionary<string, string> Title,
        int? DisplayOrder,
        string? Language,
        string? ScreenSize,
        decimal? FocusPointX,
        decimal? FocusPointY,
        string Uri,
        long? Width,
        long? Height
    );

    public record TitledImageItemSLResponse
    (
        Guid Id,
        string Title,
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
