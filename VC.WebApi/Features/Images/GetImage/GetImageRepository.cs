using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VC.WebApi.Domain.Images;
using VC.WebApi.Infrastructure.EFCore.Context;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.GetImage
{
    public class GetImageRepository(VCDbContext dbContext) : IGetImageRepository
    {
        public async Task<Result<ImageResponse>> GetImage(GetImageRequest request)
        {
            try
            {
                ImageId imageId = ImageId.CreateFromGuid(request.ImageId);

                if (request.PreferredLanguages.Count == 0)
                {
                    ImageMLResponse? imageMLResponse;

                    var query = dbContext.Image.AsNoTracking()
                                .Where(l => l.Id == imageId)
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

                    imageMLResponse = await query.SingleOrDefaultAsync();

                    if (imageMLResponse is null)
                    {
                        return Result<ImageResponse>.Failure(Error.NotFound.Not_Found_Resource_With_Id(nameof(Image), imageId.Value));
                    }
                    else
                    {
                        return Result<ImageResponse>.Success(new() { MultiLanguageResponse = imageMLResponse });
                    };
                }
                else
                {
                    string prefLangs = string.Join(",", request.PreferredLanguages);

                    ImageSLResponse? imageSLResponse;

                    var query = dbContext.Image.AsNoTracking()
                               .Where(l => l.Id == imageId)
                               //.OrderBy(l => dbContext.GetLanguage(l.Title, @prefLangs)!)
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
                               )
                               ;

                    imageSLResponse = await query.SingleOrDefaultAsync();


                    if (imageSLResponse is null)
                    {
                        return Result<ImageResponse>.Failure(Error.NotFound.Not_Found_Resource_With_Id(nameof(Image), imageId.Value));
                    }
                    else
                    {
                        return Result<ImageResponse>.Success(new() { SingleLanguageResponse = imageSLResponse });
                    };
                }
            }
            catch (SqlException)
            {
                //Handle expectable errors like deadlocks
                throw;
            }
        }
    }
}
