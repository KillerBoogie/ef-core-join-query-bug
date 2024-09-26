using VC.WebApi.Shared.Identity;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Accounts
{
    public class AccountId : GuidId
    {
        public static AccountId Anonymous()
        {
            var r = CreateFromString("00000000-0000-0000-0000-000000000000");
            return r.Value;
        }
        public static AccountId Create()
        {
            return new AccountId();
        }

        public static AccountId CreateFromGuid(Guid guid)
        {
            return new AccountId(guid);
        }

        public static Result<AccountId> CreateFromString(string? text)
        {
            Result<Guid> result = Parse<AccountId>(text);
            return result.IsSuccess ?
                Result<AccountId>.Success(new AccountId(result.Value)) :
                Result<AccountId>.Failure(result);
        }

        private AccountId(Guid guid) : base(guid) { }
        private AccountId() : base() { }

    }
}
