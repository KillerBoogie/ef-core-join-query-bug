using System.Globalization;
using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Languages
{
    public partial class Language : ValueObject, ICreate<Language, string>
    {
        public string Tag { get; }

        public static readonly int MaxLength = 14;
        public static readonly int MinLength = 2;

#pragma warning disable CS8618
        private Language()
#pragma warning restore CS8618
        { }
        private Language(string tag)
        {
            Tag = tag;
        }
        public static Result Validate(string tag)
        {
            //The max possible BCP47 tag is 35. Windows supported languages have min of 2 chars and max of 14 chars. 
            //TODO: possible languages should be read from a user changeable settings file

            if (IsCulture(tag))
                return Result.Success();
            else
                return Result.Failure(Error.Validation.Invalid_System_Language(tag));

        }
        public static Result<Language> Create(string tag)
        {
            Result result = Validate(tag);
            if (result.IsFailure)
                return Result<Language>.Failure(result);
            else
                return Result<Language>.Success(new Language(tag));
        }

        public static Language CreateFromCultureInfo(CultureInfo cultureInfo)
        {
            return new Language(cultureInfo.Name);
        }

        public override string ToString()
        {
            return Tag;
        }

        private static bool IsCulture(string tag)
        {
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(tag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Tag;
        }

    }
}
