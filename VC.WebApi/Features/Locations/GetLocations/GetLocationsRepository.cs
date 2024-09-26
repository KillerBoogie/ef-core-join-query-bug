using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VC.WebApi.Domain.ImageItems;
using VC.WebApi.Features.Addresses;
using VC.WebApi.Infrastructure.EFCore.Context;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.Results;
using VC.WebApi.SharedDtos;

namespace VC.WebApi.Features.Locations.GetLocations
{
    public class GetLocationsRepository(VCDbContext dbContext) : IGetLocationsRepository
    {
        public async Task<Result<LocationsResponse>> GetLocations(List<Language> preferredLanguages)
        {
            try
            {
                LocationsResponse response;

                if (preferredLanguages.Count == 0)
                {
                    List<LocationMLResponse> Locations;

                    //If imageId in image does not exists it throws error 
                    //TODO: handle gracefully
                    var query = dbContext.Location.AsNoTracking()//.AsSplitQuery()
                               .Select(location => new LocationMLResponse(
                                location.Id.Value,
                                location.Name.ToStringDictionary(),
                                location.CoverImages
                                    .Join(dbContext.Image.AsNoTracking(),
                                    danceCoverImage => danceCoverImage.ImageId,
                                    image => image.Id,
                                    (danceCoverImage, image) => new ImageItemResponse(
                                        danceCoverImage.ImageId.Value,
                                        danceCoverImage.DisplayOrder,
                                        danceCoverImage.Language == null ? null : danceCoverImage.Language.Tag,
                                        danceCoverImage.ScreenSize == null ? null : danceCoverImage.ScreenSize.Value,
                                        danceCoverImage.FocusPointX == null ? null : danceCoverImage.FocusPointX.Value,
                                        danceCoverImage.FocusPointY == null ? null : danceCoverImage.FocusPointY.Value,
                                        image.Uri.ToString(),
                                        image.MetaData.Width,
                                        image.MetaData.Height
                                        )).ToList(),
                                location.Address.MapToDto(),
                                location.Created,
                                location.LastModified,
                                location.Version
                                ));

                    Locations = await query.ToListAsync();

                    response = new() { MultiLanguageResponse = Locations };
                }
                else
                {
                    string prefLangs = string.Join(",", preferredLanguages);

                    List<LocationSLResponse> Locations;

                    var query = dbContext.Location.AsNoTracking().AsSplitQuery()
                              .OrderBy(l => dbContext.GetLanguage(l.Name, @prefLangs)!)
                              .Select(location => MapToLocationResponse.SLResponse(dbContext, location, prefLangs));

                    Locations = await query.ToListAsync();

                    response = new() { SingleLanguageResponse = Locations };
                }

                return Result<LocationsResponse>.Success(response);
            }
            catch (SqlException)
            {
                //Handle expectable errors like deadlocks
                throw;
            }
        }
    }
}
