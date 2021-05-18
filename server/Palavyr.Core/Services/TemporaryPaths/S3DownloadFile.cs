namespace Palavyr.Core.Services.TemporaryPaths
{
    
    public interface IHaveTempDirectory
    {
        public string TempDirectory { get; set; }
    }

    public interface IHaveLocalFileName
    {
        public string FileNameWithExtension { get; set; }
    }

    public interface IHaveTemporaryPath
    {
        public string TempFilePath { get; set; }
    }

    public interface IHaveBeenDownloadedFromS3 : IHaveTemporaryPath, IHaveLocalFileName, IHaveTempDirectory
    {
    }
    
    public class S3DownloadFile : IHaveBeenDownloadedFromS3
    {
        public string TempFilePath { get; set; }
        public string FileNameWithExtension { get; set; }
        public string TempDirectory { get; set; }
    }
}