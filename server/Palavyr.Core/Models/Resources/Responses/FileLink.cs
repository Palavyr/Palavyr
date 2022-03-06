namespace Palavyr.Core.Models.Resources.Responses
{
    public class FileLink
    {
        public string? Link { get; set; }
        public string FileName { get; set; }
        public string FileId { get; set; }

        public FileLink()
        {
            
        }
        public FileLink(string fileName, string link, string fileId)
        {
            FileName = fileName;
            Link = link;
            FileId = fileId;
        }

        public static FileLink CreateS3Link(string fileName, string fileId)
        {
            return new FileLink(fileName, "", fileId);

        }

        public static FileLink CreateUrlLink(string fileName, string link, string fileId)
        {
            return new FileLink(fileName, link, fileId);
        }
    }
}