using MediatR;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Locations.GetLocations
{
    public class GetLocationsQuery(GetLocationsRequest request,
        EnvironmentDTO Environment) : IRequest<Result<LocationsResponse>>
    {
        public GetLocationsRequest Request = request;
        public EnvironmentDTO Environment = Environment;
    }
}
