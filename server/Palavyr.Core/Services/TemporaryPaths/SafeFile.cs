namespace Palavyr.Core.Services.TemporaryPaths
{
    public class SafeFile
    {
        public string FileStem { get; set; } = null!;
        public string FileNameWithExtension { get; set; } = null!;
        public string TempDirectory { get; set; } = null!;
        public string S3Key { get; set; } = null!;
    }
}