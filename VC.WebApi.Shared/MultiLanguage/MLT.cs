using System.Collections;
using System.Text.Json;
using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Texts;

namespace VC.WebApi.Shared.MultiLanguage
{
    //MLString inherits from ValueObject for Equals, but will be changed.
    public class ML<T> : ValueObject, IEnumerable<KeyValuePair<Language, T>>
    {
        //sorted to have serializations in same order and to have clear value comparison
        private SortedDictionary<Language, T> _values = [];
        public SortedDictionary<Language, T> Values => new(_values);

        public int Count { get { return _values.Count; } }


        public ML() { }

        public ML(Language language, T value)
        {
            _values = new SortedDictionary<Language, T>() { { language, value } };
        }

        public Dictionary<string, string> ToStringDictionary()
        {
            return _values.ToDictionary(
           kvp => kvp.Key.ToString() ?? string.Empty,
           kvp => kvp.Value?.ToString() ?? string.Empty
            );
        }

        #region  Getters 

        /// <summary>
        /// Get MLItem&lt;T&gt; element of the language
        /// </summary>
        /// <param name="languages"></param>
        /// <returns>MLItem&lt;T&gt; for the given language, when there is no item for the given language
        /// the first item or null when ML is empty</returns>
        public MLItem<T>? Get(Language language)
        {
            return Get(new List<Language> { language });
        }

        /// <summary>
        /// Get MLItem&lt;T&gt; element of the language
        /// </summary>
        /// <param name="languages"></param>
        /// <returns>MLItem&lt;T&gt; for the given language, when there is no item for the given language
        /// the first item or null when ML is empty</returns>
        public MLItem<T>? GetExact(Language language)
        {
            MLItem<T>? value = GetPreferredOnly(language);

            return value;
        }

        /// <summary>
        /// Get MLItem&lt;T&gt; element of the first language available in the Language-List
        /// </summary>
        /// <param name="languages"></param>
        /// <returns>MLItem&lt;T&gt; of found language otherwise first MLItem&lt;T&gt; element
        /// or null when ML is empty</returns>
        public virtual MLItem<T>? Get(IList<Language> languages)
        {
            // return doesn't use Result type, because null can be a valid return value if entry does not exists
            // if field is nullable this has to be decided in using code

            MLItem<T>? value = GetPreferredOnly(languages);

            if (value is not null)
            {
                return value;
            }
            else
            {
                return GetAny();
            }
        }


        /// <summary>
        /// Get MLItem&lt;T&gt; for the given language
        /// </summary>
        /// <returns>MLItem&lt;T&gt; of found language or null</returns>
        private MLItem<T>? GetPreferredOnly(Language language)
        {
            return GetPreferredOnly(new List<Language> { language });
        }


        /// <summary>
        /// Get MLItem&lt;T&gt; of the first language available in the Language-List
        /// </summary>
        /// <returns>MLItem&lt;T&gt; of found language or null</returns>
        private MLItem<T>? GetPreferredOnly(IList<Language> languages)
        {
            if (languages.Count == 0)
            {
                return null;
            }

            foreach (Language lang in languages)
            {
                bool found = _values.TryGetValue(lang, out T? value);
                if (found)
                {
                    return new MLItem<T>(lang, value!); //T is not null in dict.
                }
            }

            return null;
        }


        /// <summary>
        /// Get any MLItem&lt;T&gt; element
        /// </summary>
        /// <returns>First MLItem&lt;T&gt; or null when ML is empty</returns>
        private MLItem<T>? GetAny()
        {
            KeyValuePair<Language, T>? entry = _values.FirstOrDefault();
            if (entry is null)
            {
                return null;
            }
            else
            {
                return new MLItem<T>(entry.Value.Key, entry.Value.Value);
            }
        }


        #endregion

        #region setters
        /// <summary>
        /// The value for the given language is added or replaced
        /// </summary>
        public void Add(Language language, T value)
        {
            _values[language] = value;
        }


        /// <summary>
        /// The entry for the given language is removed, If the last entry is removed
        /// an empty ML-object is returned.
        /// </summary>
        /// <returns>Result.Success if entry is sucessfully removed
        /// <para />Result.Failure if no entry for the language exists</returns>
        public virtual Result RemoveLanguage(Language language)
        {
            // empty MLString is allowed
            if (_values.Remove(language))
                return Result.Success();
            else
                return Result.Failure(Error.Validation.ML_Cant_Remove_Language_Does_Not_Exists(language));
        }

        #endregion

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new MLJsonConverterFactory(), new TextJsonConverterFactory() } });
        }

        // Implementation of IEnumerable<KeyValuePair<Language, T>>
        public IEnumerator<KeyValuePair<Language, T>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public static Result<ML<U>> Create<U>(Dictionary<string, string> ml) where U : ICreate<U, string>
        {
            ErrorList errors = new();

            ML<U>? mlText = [];

            foreach (KeyValuePair<string, string> item in ml)
            {
                Result<Language> resultLanguage = Language.Create(item.Key);
                Result<U> resultT = U.Create(item.Value);

                errors.AddIfFailure(resultLanguage);
                errors.AddIfFailure(resultT);

                if (resultT.IsSuccess && resultLanguage.IsSuccess)
                {
                    mlText.Add(resultLanguage.Value, resultT.Value);
                }
            }

            return errors.HasErrors
            ? Result<ML<U>>.Failure(errors)
            : Result<ML<U>>.Success(mlText);
        }
    }
}
