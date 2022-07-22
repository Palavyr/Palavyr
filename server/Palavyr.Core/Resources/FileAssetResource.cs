namespace Palavyr.Core.Resources
{
    public class FileAssetResource
    {
        public string FileName { get; set; } // the risky Name with extension
        public string FileId { get; set; } // the file id
        public string Link { get; set; } // a link to the file (local or cloud)
    }
}