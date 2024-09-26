using System.Security.Cryptography;
using System.Text;

public static class ETagHelper
{
    public static string GenerateETag(string data)
    {
        using (var md5 = MD5.Create())
        {
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hash);
        }
    }
}