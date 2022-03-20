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

    public interface IHaveBeenDownloadedFromCloudToLocal : IHaveTemporaryPath, IHaveLocalFileName, IHaveTempDirectory
    {
    }
    
    public class S3DownloadFile : IHaveBeenDownloadedFromCloudToLocal
    {
        public string TempFilePath { get; set; }
        public string FileNameWithExtension { get; set; }
        public string TempDirectory { get; set; }
    }
}