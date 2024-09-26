using VC.WebApi.Shared.Accounts;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;


namespace VC.WebApi.Shared.Auditing
{

    public class CreatedInfo
    {
        public DateTime Created { get; init; }
        public AccountId CreatedBy { get; init; }
        public AccountId CreatedInNameOf { get; init; }

        public static readonly DateTime MinSystemDate = new(1950, 1, 1);

        private CreatedInfo(DateTime created, AccountId createdBy, AccountId createdInNameOf)
        {
            Created = created;
            CreatedBy = createdBy;
            CreatedInNameOf = createdInNameOf;
        }

        public static Result<CreatedInfo> Create(DateTime created, AccountId createdBy, AccountId createdInNameOf)
        {
            Result result = Validate(created);
            return result.IsSuccess ?
                    Result<CreatedInfo>.Success(new CreatedInfo(created, createdBy, createdInNameOf)) :
                    Result<CreatedInfo>.Failure(result);
        }

        public static Result Validate(DateTime lastUpdated)
        {
            //TDOO: inject Now
            if (lastUpdated > DateTime.UtcNow)
                return Result.Failure(Error.Validation.Date_Cant_Be_In_Future(nameof(lastUpdated)));

            if (lastUpdated < MinSystemDate)
                return Result.Failure(Error.Validation.Date_Cant_Be_Before_Min_Date(nameof(lastUpdated), MinSystemDate));

            return Result.Success();
        }
    }
}
