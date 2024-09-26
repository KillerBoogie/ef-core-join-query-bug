using VC.WebApi.Shared.BaseInterfaces;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;
using VC.WebApi.Shared.Texts;

namespace VC.WebApi.Shared.Files
{
    // Diese Klasse ist der technische Name eines hochgeladenen/verfügbaren Images/Videos
    // Der Name wird auch verwendet um auf das Image/Video durch eine Uri zuzugreifen.
    //
    // Als technischen Namen könnte man auch eine UUID in dem Fall die ImageId verwenden
    // dies würde aber "Reparaturen" auf dem Filesysem erschweren bzw. verhindern.
    // Der Vorteil  ist dass die URIs nicht, nur schwer erraten werden können, was einen
    // besseren Schutz für die Bilder bietet und daher sollte auch keine sequential-UUID verwendet werden.
    // Da die URIs aber nur von der React-App angesprochen werden und gegen Zugriff von außen
    // geschützt sind, spielt dieser Datenschutzaspekt nur eine untergeordnete Rolle.
    //
    // ==> lesbare Namen sind ok
    //
    public class FileName : Text, ICreate<FileName, string>
    {
        // see https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file
        new public static int MinLength => 1;
        new public static int MaxLength => 256;
        private FileName(string value, int minLength, int maxLength, string fieldName) : base(value, minLength, maxLength, fieldName) { }

        public static Result<FileName> Create(string text)
        {
            ErrorList errors = new();
            Result result = Validate(text, MinLength, MaxLength, nameof(FileName));
            errors.AddIfFailure(result);

            if (text.IndexOfAny("<>:\"/\\|?*".ToCharArray()) != -1)
            {
                errors.Add(Error.Validation.Contains_Invalid_Characters(nameof(FileName)));
            }

            if (errors.HasErrors)
            {
                return Result<FileName>.Failure(errors);
            }
            else
            {
                return Result<FileName>.Success(new FileName(text, MinLength, MaxLength, nameof(FileName)));
            }
        }
    }
}
