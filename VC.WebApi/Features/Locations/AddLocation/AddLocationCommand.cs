using MediatR;
using VC.WebApi.Infrastructure.MediatR.Pipelines;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Responses;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Locations.AddLocation
{
    public class AddLocationCommand(AddLocationRequest addLocationRequest, EnvironmentDTO environmentDto) : IRequest<Result<GuidIdResponse>>, ITransactional
    {
        public EnvironmentDTO Environment = environmentDto;
        public AddLocationRequest AddLocationRequest = addLocationRequest;
    }
}
