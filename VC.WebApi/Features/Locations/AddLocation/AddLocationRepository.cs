using Microsoft.EntityFrameworkCore;
using VC.WebApi.Domain.Locations;
using VC.WebApi.Infrastructure.EFCore.Context;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;
using VC.WebApi.SharedKernel.Persistence;

namespace VC.WebApi.Features.Locations.AddLocation
{
    public class AddLocationRepository(VCDbContext dbContext) : IAddLocationRepository
    {
        public async Task<Result> AddLocation(Location location)
        {
            dbContext.Add(location);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                //Handle expectable errors like deadlocks and concurrency
                throw;
            }

            return Result.Success();
        }

        public async Task<Result> AreNamesUnique(Location location)
        {
            List<DbLocationStub> dBLocationStubs;

            try
            {
                dBLocationStubs = await dbContext.Location.ForTableLock().AsNoTracking().Select(l => new DbLocationStub(
                                l.Name.ToStringDictionary()
                                )).ToListAsync();

            }
            catch (Exception)
            {
                //Handle expectable errors like deadlocks and concurrency
                throw;
            }

            ErrorList errors = new();

            foreach (KeyValuePair<string, string> slName in location.Name.ToStringDictionary())
            {
                foreach (DbLocationStub locationStub in dBLocationStubs)
                {
                    if (locationStub.Name[slName.Key].ToLower() == slName.Value.ToLower())
                    {
                        errors.Add(Error.Validation.Unique_MLValue_Required(nameof(Location.Name), slName.Value, slName.Key));
                    };
                }
            }

            if (errors.HasErrors)
            {
                return Result.Failure(errors);
            }

            return Result.Success();

        }

        private record DbLocationStub(Dictionary<string, string> Name);

    }
}