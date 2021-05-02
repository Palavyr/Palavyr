#nullable enable
namespace Palavyr.Core.Services.AttachmentServices
{
    public class AttachmentMeta
    {
        public string S3Key { get; set; } = null!;
        public string RiskyName { get; set; } = null!;
        public string SafeFileId { get; set; } = null!;

        public string? LocalFilePath { get; set; }

        public void SetLocalFilePath(string localFilePath)
        {
            LocalFilePath = localFilePath;
        }
    }
}