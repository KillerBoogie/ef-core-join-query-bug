using VC.WebApi.Shared.Identity;

namespace VC.WebApi.Shared.Errors
{
    public static class ErrorExtensions
    {
        public static Error AddExtension(this Error error, string parameterName, GuidId parameterValue)
        {
            error.AddExtension(parameterName, parameterValue);
            return error;
        }
    }

}
