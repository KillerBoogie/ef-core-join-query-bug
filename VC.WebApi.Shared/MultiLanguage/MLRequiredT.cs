using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Languages;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.MultiLanguage
{
    public class MLRequired<T> : ML<T>
    {
        public MLRequired(Language language, T value) : base(language, value) { }

        /// <summary>
        /// Get MLItem&lt;T&gt; element of the first language available in the Language-List
        /// </summary>
        /// <returns>MLItem&lt;T&gt; of found language otherwise first MLItem&lt;T&gt; element
        /// <para />Throws NullReferenceException when MLRequired is empty</returns>
        /// <exception cref="NullReferenceException">Thrown when MLRequired is empty</exception>
        public override MLItem<T> Get(IList<Language> languages)
        {
            MLItem<T>? mlItem = base.Get(languages);

            if (mlItem is null)
            {
                throw new NullReferenceException("Get returned no value for MLRequired!");
            }
            else
            {
                return mlItem;
            }
        }

        /// <summary>
        /// The entry for the given language is removed. It is not allowed to remove
        /// the last entry.
        /// </summary>
        /// <returns>Result.Success if entry is sucessfully removed
        /// <para />Result.Failure if the last entry should be removed or no entry for the language exists</returns>
        public override Result RemoveLanguage(Language language)
        {
            if (Count == 1)
            {
                return Result.Failure(Error.Validation.ML_Cant_Remove_Language_Requires_At_Least_One_Language(language));
            }
            else
            {
                return base.RemoveLanguage(language);
            }
        }

        public static new Result<MLRequired<U>> Create<U>(Dictionary<string, string> ml) where U : ICreate<U, string>
        {
            ErrorList errors = new();

            MLRequired<U>? mlText = null;

            foreach (KeyValuePair<string, string> item in ml)
            {
                Result<Language> resultLanguage = Language.Create(item.Key);
                Result<U> resultT = U.Create(item.Value);

                errors.AddIfFailure(resultLanguage, item.Key);
                errors.AddIfFailure(resultT, item.Key);

                if (resultT.IsSuccess && resultLanguage.IsSuccess)
                {
                    if (mlText is null)
                    {
                        mlText = new(resultLanguage.Value, resultT.Value);
                    }
                    else
                    {
                        mlText.Add(resultLanguage.Value, resultT.Value);
                    }
                }
            }

            if (mlText is null)
            {
                errors.Add(Error.Validation.Must_Not_Be_Empty(typeof(U).Name));
            }

            return errors.HasErrors
            ? Result<MLRequired<U>>.Failure(errors)
            : Result<MLRequired<U>>.Success(mlText!);
        }
    }
}
