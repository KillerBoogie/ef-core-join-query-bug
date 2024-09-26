using VC.WebApi.Shared.Accounts;

namespace VC.WebApi.Shared.Auditing
{
    public abstract class AuditInfo
    {
        public DateTime Created { get; protected set; }
        public AccountId CreatedBy { get; protected set; }
        public AccountId CreatedInNameOf { get; protected set; }

        public DateTime LastModified { get; protected set; }
        public AccountId LastModifiedBy { get; protected set; }
        public AccountId LastModifiedInNameOf { get; protected set; }

#pragma warning disable CS8618
        protected AuditInfo() { }
#pragma warning restore CS8618

        protected AuditInfo(CreatedInfo createdInfo)
        {
            Created = createdInfo.Created;
            CreatedBy = createdInfo.CreatedBy;
            CreatedInNameOf = createdInfo.CreatedInNameOf;
            LastModified = createdInfo.Created;
            LastModifiedBy = createdInfo.CreatedBy;
            LastModifiedInNameOf = createdInfo.CreatedInNameOf;
        }

        protected void SetModifyInfo(ModifiedInfo modifiedInfo)
        {
            LastModified = modifiedInfo.Modified;
            LastModifiedBy = modifiedInfo.ModifiedBy;
            LastModifiedInNameOf = modifiedInfo.ModifiedInNameOf;
        }
    }
}
