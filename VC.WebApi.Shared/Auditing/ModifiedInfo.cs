using VC.WebApi.Shared.Accounts;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;


namespace VC.WebApi.Shared.Auditing
{

    public class ModifiedInfo
    {
        public DateTime Modified { get; init; }
        public AccountId ModifiedBy { get; init; }
        public AccountId ModifiedInNameOf { get; init; }

        public static readonly DateTime MinSystemDate = new(1950, 1, 1);

        private ModifiedInfo(DateTime modified, AccountId modifiedBy, AccountId modifiedInNameOf)
        {
            Modified = modified;
            ModifiedBy = modifiedBy;
            ModifiedInNameOf = modifiedInNameOf;
        }

        public static Result<ModifiedInfo> Create(DateTime modified, AccountId modifiedBy, AccountId modifiedInNameOf)
        {
            Result result = Validate(modified);
            return result.IsSuccess ?
                    Result<ModifiedInfo>.Success(new ModifiedInfo(modified, modifiedBy, modifiedInNameOf)) :
                    Result<ModifiedInfo>.Failure(result);
        }

        public static Result Validate(DateTime lastUpdated)
        {
            if (lastUpdated > DateTime.UtcNow)
                return Result.Failure(Error.Validation.Date_Cant_Be_In_Future(nameof(lastUpdated)));

            if (lastUpdated < MinSystemDate)
                return Result.Failure(Error.Validation.Date_Cant_Be_Before_Min_Date(nameof(lastUpdated), MinSystemDate));

            return Result.Success();
        }
    }
}
