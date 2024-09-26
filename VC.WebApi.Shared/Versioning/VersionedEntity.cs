using VC.WebApi.Shared.Auditing;
using VC.WebApi.Shared.DDD;

namespace VC.WebApi.Shared.Versioning
{
    public abstract class VersionedEntity<TId> : Entity<TId>
    {
        public ulong Version { get; protected set; }

#pragma warning disable CS8618
        protected VersionedEntity() { }
#pragma warning restore CS8618

        public VersionedEntity(TId id, CreatedInfo createdInfo) : base(id, createdInfo)
        {
            Version = 0;
        }
    }
}
