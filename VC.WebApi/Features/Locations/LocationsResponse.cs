namespace VC.WebApi.Features.Locations
{
    // Single Language filter is done in SQL request on server
    public record LocationsResponse(
       List<LocationSLResponse>? SingleLanguageResponse = null,
       List<LocationMLResponse>? MultiLanguageResponse = null
    )
    {
        public bool HasValue()
        {
            return SingleLanguageResponse is not null || MultiLanguageResponse is not null;
        }
    };
}
