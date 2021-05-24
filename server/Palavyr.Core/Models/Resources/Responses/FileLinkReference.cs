namespace Palavyr.Core.Models.Resources.Responses
{
    public class FileLinkReference
    {
        public string FileReference { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }

        private FileLinkReference(string fileName, string fileReference, string fileId)
        {
            FileReference = fileReference;
            FileName = fileName;
            FileId = fileId;
        }

        public static FileLinkReference CreateLink(string fileName, string fileReference, string fileId)
        {
            return new FileLinkReference(fileName, fileReference, fileId);
        }

        public static FileLinkReference CreateEmptyLink()
        {
            return new FileLinkReference("No Pdf Sent", "", "");
        }
    }
}