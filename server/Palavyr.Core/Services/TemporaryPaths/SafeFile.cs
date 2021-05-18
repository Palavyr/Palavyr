namespace Palavyr.Core.Services.TemporaryPaths
{
    public class SafeFile
    {
        public string FileStem { get; set; }
        public string FileNameWithExtension { get; set; }
        public string TempDirectory { get; set; }
        public string S3Key { get; set; }
    }
}