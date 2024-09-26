using MediatR;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Locations.GetLocations
{
    public class GetLocationsHandler(IGetLocationsRepository getLocationsRepository)
        : IRequestHandler<GetLocationsQuery, Result<LocationsResponse>>
    {
        public async Task<Result<LocationsResponse>> Handle(GetLocationsQuery query, CancellationToken cancellationToken)
        {
            List<Language> langs = query.Request.PreferredLanguages;

            return await getLocationsRepository.GetLocations(langs);
        }
    }
}
