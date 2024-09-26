using System.Text.Json;
using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.MultiLanguage;

namespace VC.WebApi.Shared.Texts
{
    public class TextList<T> : ValueObject
    {

        private SortedSet<T> _values = new();
        public int Count { get { return _values.Count; } }

        public TextList() { }
        public TextList(T value)
        {
            _values = new SortedSet<T>() { { value } };
        }

        public TextList(SortedSet<T> values)
        {
            _values = values;
        }

        public void Add(T value)
        {
            _values.Add(value);
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return JsonSerializer.Serialize(_values, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new MLJsonConverterFactory(), new TextJsonConverterFactory() } });
        }
    }
}
