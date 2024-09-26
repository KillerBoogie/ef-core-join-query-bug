using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.BaseInterfaces
{
    public interface ICreate<T, V>
    {
        public abstract static Result<T> Create(V value);
    }
}
