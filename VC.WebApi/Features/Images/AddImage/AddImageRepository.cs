using Microsoft.EntityFrameworkCore;
using VC.WebApi.Domain.Images;
using VC.WebApi.Infrastructure.EFCore.Context;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;
using VC.WebApi.SharedKernel.Persistence;

namespace VC.WebApi.Features.Images.AddImage
{
    public class AddImageRepository(VCDbContext dbContext) : IAddImageRepository
    {
        public async Task<Result> AddImage(Image image)
        {
            dbContext.Add(image);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //Handle expectable errors like deadlocks and concurrency
                Result result = Result.Failure(Error.Fatal.Error(ex.Message));
                return result;
            }

            return Result.Success();
        }

        public async Task<Result> IsUriUnique(Uri uri)
        {
            bool isUnique = false;

            try
            {
                isUnique = !await dbContext.Image.ForTableLock().AnyAsync(d => d.Uri == uri);

            }
            catch (Exception)
            {
                //Handle expectable errors like deadlocks and concurrency
                throw;
            }
            if (!isUnique)
            {
                return Result.Failure(Error.Validation.Unique_Value_Required(nameof(Image.Uri), uri.ToString()));
            }
            return Result.Success();
        }
    }
}