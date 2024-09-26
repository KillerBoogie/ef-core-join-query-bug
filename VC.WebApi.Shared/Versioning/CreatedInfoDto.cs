using VC.WebApi.Shared.Accounts;

namespace VC.WebApi.Shared.Versioning
{
    public record CreatedInfoDto
    (
        DateTime Created,
        AccountId CreatedBy,
        AccountId CreatedInNameOf
    );
}
