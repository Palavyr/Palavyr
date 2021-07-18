namespace Palavyr.Core.Models.Resources.Responses
{
    public class FileLink
    {
        public string? Link { get; set; }
        public string FileName { get; set; }
        public string FileId { get; set; }
        public bool IsUrl { get; set; }
        public string S3Key { get; set; }

        private FileLink(string fileName, string link, string fileId, string s3Key, bool isUrl)
        {
            FileName = fileName;
            Link = link;
            FileId = fileId;
            IsUrl = isUrl;
            S3Key = s3Key;
        }

        public static FileLink CreateS3Link(string fileName, string fileId, string s3Key)
        {
            return new FileLink(fileName, "", fileId, s3Key, false);
        }

        public static FileLink CreateUrlLink(string fileName, string link, string fileId)
        {
            return new FileLink(fileName, link, fileId, "", false);
        }
    }
}