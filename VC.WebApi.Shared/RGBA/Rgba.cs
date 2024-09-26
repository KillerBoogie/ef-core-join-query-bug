using System.Text.RegularExpressions;
using VC.WebApi.Shared.DDD;
using VC.WebApi.Shared.Errors;
using VC.WebApi.Shared.Results;

namespace VC.WebApi.Shared.Color
{
    public class Rgba : ValueObject
    {
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }
        public byte A { get; private set; }

        private Rgba(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static Result<Rgba> CreateFromString(string rgbaString)
        {
            // Regular expression to match rgba(r, g, b, a) format
            var match = Regex.Match(rgbaString, @"rgba\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(0|1|0?\.\d+)\s*\)");

            if (!match.Success)
                return Result<Rgba>.Failure(Error.Validation.Invalid_RGBA_Format());

            // Parsing the matched values
            byte r = byte.Parse(match.Groups[1].Value);
            byte g = byte.Parse(match.Groups[2].Value);
            byte b = byte.Parse(match.Groups[3].Value);
            float alpha = float.Parse(match.Groups[4].Value);

            // Converting alpha from 0.0-1.0 to 0-255
            byte a = (byte)(alpha * 255);

            return Result<Rgba>.Success(new Rgba(r, g, b, a));
        }

#pragma warning disable CS8618
        private Rgba() { }
#pragma warning restore CS8618

        public override string ToString()
        {
            return $"RGBA({R}, {G}, {B}, {A})";
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ToString();
        }
    }
}
