using Microsoft.EntityFrameworkCore;
using VC.WebApi.Domain.Locations;
using VC.WebApi.Features.Addresses;
using VC.WebApi.Infrastructure.EFCore.Context;
using VC.WebApi.SharedDtos;

namespace VC.WebApi.Features.Locations
{
    internal static class MapToLocationResponse
    {
        public static LocationMLResponse MLResponse(VCDbContext dbContext, Location location)
        {
            return new LocationMLResponse(
                location.Id.Value,
                location.Name.ToStringDictionary(),
                location.CoverImages
                    .Join(dbContext.Image.AsNoTracking(),
                    danceCoverImage => danceCoverImage.ImageId,
                    image => image.Id,
                    (danceCoverImage, image) => Mappings.MapToImageItemResponse(danceCoverImage, image)).ToList(),
                location.Address.MapToDto(),
                location.Created,
                location.LastModified,
                location.Version
                );
        }

        public static LocationSLResponse SLResponse(VCDbContext dbContext, Location location, string prefLangs)
        {
            return new LocationSLResponse(
                location.Id.Value,
                dbContext.GetLanguage(location.Name, @prefLangs)!,
                location.CoverImages
                    .Join(dbContext.Image.AsNoTracking(),
                    danceCoverImage => danceCoverImage.ImageId,
                    image => image.Id,
                    (danceCoverImage, image) => Mappings.MapToImageItemResponse(danceCoverImage, image)).ToList(),
                location.Address.MapToDto(),
                location.Created,
                location.LastModified,
                location.Version
                );
        }
    }
}