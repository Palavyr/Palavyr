namespace Palavyr.Core.Models.Resources.Responses
{
    public class FileLink
    {
        public string Link { get; set; }
        public string FileName { get; set; }
        public string FileId { get; set; }
        public bool IsUrl { get; set; }
        
        private FileLink(string fileName, string link, string fileId, bool isUrl)
        {
            FileName = fileName;
            Link = link;
            FileId = fileId;
            IsUrl = isUrl;
        }

        public static FileLink CreateLink(string fileName, string link, string fileId, bool isUrl = false)
        {
            return new FileLink(fileName, link, fileId, isUrl);
        }
    }
}