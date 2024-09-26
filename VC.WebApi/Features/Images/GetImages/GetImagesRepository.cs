using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VC.WebApi.Infrastructure.EFCore.Context;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.GetImages
{
    public class GetImagesRepository(VCDbContext dbContext) : IGetImagesRepository
    {
        public async Task<Result<ImagesResponse>> GetImages(List<Language> preferredLanguages)
        {
            try
            {
                ImagesResponse response;

                if (preferredLanguages.Count == 0)
                {
                    List<ImageMLResponse> Images;

                    var query = dbContext.Image.AsNoTracking()
                                .Select(l => new ImageMLResponse(
                                    l.Id.Value,
                                    l.Uri.ToString(),
                                    l.FileName.ToString(),
                                    l.Description.ToStringDictionary(),
                                    l.MetaData.Size,
                                    l.MetaData.Width,
                                    l.MetaData.Height,
                                    l.Created,
                                    l.LastModified,
                                    l.Version
                                    )
                                );

                    Images = await query.ToListAsync();
                    response = new() { MultiLanguageResponse = Images };
                }
                else
                {
                    string prefLangs = string.Join(",", preferredLanguages);

                    List<ImageSLResponse> images;

                    var query = dbContext.Image.AsNoTracking()
                              .Select(l => new ImageSLResponse(
                                  l.Id.Value,
                                  l.Uri.ToString(),
                                  l.FileName.ToString(),
                                  dbContext.GetLanguage(l.Description, @prefLangs)!,
                                  l.MetaData.Size,
                                  l.MetaData.Width,
                                  l.MetaData.Height,
                                  l.Created,
                                  l.LastModified,
                                  l.Version
                                  )
                              );

                    images = await query.ToListAsync();
                    response = new() { SingleLanguageResponse = images };
                }

                return Result<ImagesResponse>.Success(response);
            }
            catch (SqlException)
            {
                //Handle expectable errors like deadlocks
                throw;
            }
        }
    }
}
