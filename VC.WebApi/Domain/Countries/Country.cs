using VC.WebApi.Shared.Auditing;
using VC.WebApi.Shared.MultiLanguage;
using VC.WebApi.Shared.Versioning;

namespace VC.WebApi.Domain.Countries
{
    public class Country : VersionedEntity<CountryId>
    {
        public MLRequired<CountryName> Name { get; private set; }

#pragma warning disable CS8618
        private Country()
        { }
#pragma warning restore CS8618

        private Country(CountryId id, MLRequired<CountryName> name, CreatedInfo createdInfo) : base(id, createdInfo)
        {
            Name = name;
        }

        public static Country Create(CountryId id, MLRequired<CountryName> name, CreatedInfo auditInfo)
        {
            return new(id, name, auditInfo);
        }
    }
}
