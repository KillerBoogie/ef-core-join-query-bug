using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Identity
{

    public abstract class GuidId : ValueObject
    {
        public Guid Value { get; }

        protected GuidId(Guid guid)
        {
            Value = guid;
        }

        protected GuidId() : this(Guid.NewGuid())
        {

        }

        protected static Result<Guid> Parse<T>(string? text)
        {
            Guid guid;
            if (string.IsNullOrWhiteSpace(text)) return Result<Guid>.Failure(Error.Validation.Must_Not_Be_Empty(nameof(GuidId)).AddExtension(nameof(T), text));

            return Guid.TryParse(text, out guid) ?
                Result<Guid>.Success(guid) :
                Result<Guid>.Failure(Error.Validation.Invalid_Guid(nameof(GuidId)).AddExtension(nameof(T), text));
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.ToString();
        }
    }
}
