using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VC.WebApi.Infrastructure.Middleware.Attributes
{
    //https://github.com/filipw/Strathweb.Samples.AspNetCore.QueryStringBinding/tree/master/Strathweb.Samples.AspNetCore.QueryStringBinding
    //refactored to use List to store and receive, because it is much faster for a few values
    //refactored to work with nullable
    public class SeparatedQueryStringValueProviderFactory : IValueProviderFactory
    {
        private readonly string _separator;
        private List<string> _keys = new();

        public SeparatedQueryStringValueProviderFactory(string separator) : this(new List<string>(), separator)
        { }

        public SeparatedQueryStringValueProviderFactory(string key, string separator) : this(new List<string> { key }, separator)
        {
        }

        public SeparatedQueryStringValueProviderFactory(IEnumerable<string> keys, string separator)
        {
            _keys.AddRange(keys);
            _separator = separator;
        }

        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            context.ValueProviders.Insert(0,
                new SeparatedQueryStringValueProvider(_keys, context.ActionContext.HttpContext.Request.Query,
                    _separator));
            return Task.CompletedTask;
        }

        public void AddKey(string key)
        {
            _keys.Add(key);
        }
    }
}