using MediatR;
using VC.WebApi.Domain.ImageItems;
using VC.WebApi.Domain.Locations;
using VC.WebApi.Infrastructure.Middleware.ModelBinder;
using VC.WebApi.Shared.Addresses;
using VC.WebApi.Shared.Auditing;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Responses;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Locations.AddLocation
{
    public class AddLocationHandler(IAddLocationRepository AddLocationRepository)
        : IRequestHandler<AddLocationCommand, Result<GuidIdResponse>>
    {
        public async Task<Result<GuidIdResponse>> Handle(AddLocationCommand command, CancellationToken cancellationToken)
        {
            EnvironmentDTO environment = command.Environment;
            AddLocationRequest request = command.AddLocationRequest;

            DateTime utcNow = DateTime.UtcNow;

            Result<Location> locationResult = CreateLocationFromDto(request, environment, utcNow);
            if (locationResult.IsFailure)
            {
                return Result<GuidIdResponse>.Failure(locationResult);
            }

            Location location = locationResult.Value;

            Result areUniqueResult = await AddLocationRepository.AreNamesUnique(location);

            if (areUniqueResult.IsFailure)
            {
                return Result<GuidIdResponse>.Failure(areUniqueResult);
            }

            Result result = await AddLocationRepository.AddLocation(location);

            if (result.IsFailure)
            {
                return Result<GuidIdResponse>.Failure(result);
            }

            return Result<GuidIdResponse>.Success(new GuidIdResponse(location.Id.Value));
        }

        private static Result<Location> CreateLocationFromDto(AddLocationRequest request, EnvironmentDTO environment, DateTime utcNow)
        {
            ErrorList errors = new();

            Result<MLRequired<LocationName>> nameResult =
                MLRequired<LocationName>.Create<LocationName>(request.Name);
            errors.AddIfFailure(nameResult);

            AddressRequest addr = request.Address;
            Result<Address> addressResult = Address.Create(addr.DeliveryInstruction, addr.Street, addr.StreetNumber, addr.StreetAffix, addr.ZipCode, addr.City, addr.State, addr.CountryId, addr.CountryName);
            errors.AddIfFailure(addressResult);

            //CoverImages
            List<ImageItem> coverImages = [];
            foreach (ImageItemRequest imageRequest in request.CoverImages)
            {
                Result<ImageItem> imageResult = imageRequest.CreateImageItem();
                errors.AddIfFailure(imageResult);
                if (imageResult.IsSuccess)
                {
                    coverImages.Add(imageResult.Value);
                }
            }

            Result<CreatedInfo> createdInfoResult = CreatedInfo.Create(utcNow, environment.Actor, environment.ActingInNameOf);
            errors.AddIfFailure(createdInfoResult);

            if (errors.HasErrors)
                return Result<Location>.Failure(errors);

            LocationId locationId = LocationId.Create();

            Result<Location> locationResult = Location.Create(locationId, nameResult.Value, addressResult.Value, coverImages, createdInfoResult.Value);

            if (locationResult.IsFailure)
            {
                return Result<Location>.Failure(locationResult);
            }

            return Result<Location>.Success(locationResult.Value);
        }
    }
}
