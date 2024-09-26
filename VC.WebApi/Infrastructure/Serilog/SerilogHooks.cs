
using Serilog.Sinks.File.Archive;
using System.IO.Compression;

namespace VC.WebApi.Infrastructure.Serilog
{
    public class SerilogHooks
    {
        public static ArchiveHooks MyArchiveHooks => new ArchiveHooks(CompressionLevel.SmallestSize, "D:\\git\\vc-web-api\\Logs");
    }
}
