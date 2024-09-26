using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VC.WebApi.Domain.Images;
using VC.WebApi.Infrastructure.EFCore.Context;
using VC.WebApi.Shared.EFCore;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Features.Images.DeleteImage
{
    public class DeleteImageRepository(VCDbContext dbContext) : IDeleteImageRepository
    {
        public async Task<Result> DeleteImage(ImageId imageId)
        {
            int deletedRowsCount = -1;
            try
            {
                deletedRowsCount = await dbContext.Image.Where(d => d.Id == imageId).ExecuteDeleteAsync();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // Extract relevant information from the error message
                    var errorMessage = ex.Message;
                    (string constraintName, string databaseName, string referencingTable, string columnName) = ErrorMessageParser.ParseFKMessage(errorMessage);

                    ErrorList errors = new();

                    string referencingResource = ErrorMessageParser.MapToResource(referencingTable);

                    errors.Add(Error.Conflict.No_Deletion_For_Linked_Resource(
                            nameof(Image), referencingResource,
                            referencingTable, constraintName)
                    );

                    return Result.Failure(errors);
                }
                else
                {
                    // Handle other DbUpdateExceptions
                    throw;
                }
            }
            catch (Exception)
            {
                //Handle expectable errors like foreign key delete violation, deadlocks and concurrency
                throw;
            }
            if (deletedRowsCount == 1)
            {
                return Result.Success();
            }
            else if (deletedRowsCount == 0)
            {
                return Result.Failure(Error.NotFound.Not_Found_Resource_With_Id(nameof(Image), imageId.Value));
            }
            else
            {
                //todo: span transaction in MEdiatR pipleline that is rolledback when count<>1
                throw new ApplicationException($"Delete Image by id resulted in {deletedRowsCount} deleted rows.");
            }
        }
    }
}
