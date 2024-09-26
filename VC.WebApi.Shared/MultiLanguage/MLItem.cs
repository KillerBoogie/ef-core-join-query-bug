using VC.WebApi.Shared.Languages;

namespace VC.WebApi.Shared.MultiLanguage
{
    public record MLItem<T>
        (
        Language Language,
        T Value
    );
}
