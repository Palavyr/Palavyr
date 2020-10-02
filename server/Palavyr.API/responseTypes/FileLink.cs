namespace Palavyr.API.ResponseTypes
{
    public class FileLink
    {
        public string Link { get; set; }
        public object FileName { get; set; }
        public string FileId { get; set; }
        
        private FileLink(string fileName, string link, string fileId)
        {
            FileName = fileName;
            Link = link;
            FileId = fileId;
        }
        
        public static FileLink CreateLink(string fileName, string link, string fileId)
        {
            return new FileLink(fileName, link, fileId);
        }
    }
}