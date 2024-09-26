namespace VC.WebApi.Shared.String
{
    public static class StringExtensions
    {
        public static string EmptyToNull(this string value)
        {
            return value == "" ? "null" : value;
        }
    }
}
