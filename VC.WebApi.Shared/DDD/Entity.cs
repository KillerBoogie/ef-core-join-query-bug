using VC.WebApi.Shared.Auditing;

namespace VC.WebApi.Shared.DDD
{
    public abstract class Entity<TID> : AuditInfo
    {
        public TID Id { get; protected set; }

#pragma warning disable CS8618
        protected Entity() { }
#pragma warning restore CS8618

        protected Entity(TID id, CreatedInfo createdInfo) : base(createdInfo)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TID> other) // null is not Enity<TId> == true
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id == null || other.Id == null) return false; //added check for compiler nullable

            if (Id.Equals(default(TID)) || other.Id.Equals(default(TID))) //default(int) = 0, which might be valid
                return false;

            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TID> a, Entity<TID> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<TID> a, Entity<TID> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (ToString() + Id).GetHashCode();
        }
    }
}
