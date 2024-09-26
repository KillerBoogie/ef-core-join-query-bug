using VC.WebApi.Shared.DDD;

namespace VC.WebApi.Shared.Addresses
{
    public abstract class ContactInfo : ValueObject
    {
        public abstract override string ToString();
    }
}