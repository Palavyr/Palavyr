namespace Palavyr.Core.Resources
{
    public class FileLinkResource
    {
        public string? Link { get; set; }
        public string FileName { get; set; }
        public string FileId { get; set; }

        public FileLinkResource()
        {
        }

        public FileLinkResource(string fileName, string link, string fileId)
        {
            FileName = fileName;
            Link = link;
            FileId = fileId;
        }
    }
}