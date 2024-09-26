﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace VC.WebApi.Infrastructure.Middleware.Attributes
{
    public class SeparatedQueryStringValueProvider : QueryStringValueProvider
    {

        private readonly List<string> _keys;
        private readonly string _separator;
        private readonly IQueryCollection _values;

        //public SeparatedQueryStringValueProvider(IQueryCollection values, CultureInfo culture)
        //    : base(null, values, culture)
        //{
        //}

        public SeparatedQueryStringValueProvider(string key, IQueryCollection values, string separator)
            : this(new List<string> { key }, values, separator)
        {
        }

        public SeparatedQueryStringValueProvider(IEnumerable<string> keys, IQueryCollection values, string separator)
            : base(BindingSource.Query, values, CultureInfo.InvariantCulture)
        {
            _keys = new List<string>(keys);
            _values = values;
            _separator = separator;
        }

        public override ValueProviderResult GetValue(string key)
        {
            var result = base.GetValue(key);

            if (_keys != null && !_keys.Contains(key))
            {
                return result;
            }

            if (result != ValueProviderResult.None &&
                //TODO: workaround x!
                result.Values.Any(x => x!.IndexOf(_separator, StringComparison.OrdinalIgnoreCase) > 0))
            {
                var splitValues = new StringValues(result.Values
                    .SelectMany(x => x!.Split(new[] { _separator }, StringSplitOptions.None)).ToArray());

                return new ValueProviderResult(splitValues, result.Culture);
            }

            return result;
        }
    }
}