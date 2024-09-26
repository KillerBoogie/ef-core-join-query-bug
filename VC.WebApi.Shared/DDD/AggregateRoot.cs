using VC.WebApi.Shared.Auditing;
using VC.WebApi.Shared.Versioning;

namespace VC.WebApi.Shared.DDD
{
    public abstract class AggregateRoot<TId> : VersionedEntity<TId>
    {
        //private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        //public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

#pragma warning disable CS8618
        protected AggregateRoot() { }
#pragma warning restore CS8618

        protected AggregateRoot(TId id, CreatedInfo auditInfo) : base(id, auditInfo)
        {
        }

        //protected virtual void AddDomainEvent(IDomainEvent newEvent)
        //{
        //    _domainEvents.Add(newEvent);
        //}

        //public virtual void ClearEvents()
        //{
        //    _domainEvents.Clear();
        //}

        //public void DispatchEvents()
        //{
        //    foreach (IDomainEvent domainEvent in _domainEvents)
        //    {
        //        //DomainEvents.Dispatch(domainEvent);
        //    }
        //    ClearEvents();
        //}
    }
}
