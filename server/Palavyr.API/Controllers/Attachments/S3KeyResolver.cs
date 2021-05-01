using System.IO;

namespace Palavyr.API.Controllers.Attachments
{
    public interface IS3KeyResolver
    {
        string ResolveAttachmentKey(string account, string areaId, string safeFileName);
    }

    public class S3KeyResolver : IS3KeyResolver
    {
        public string ResolveAttachmentKey(string account, string areaId, string safeFileName)
        {
            return Path.Combine(account, "AreaData", areaId, "Attachments", safeFileName + ".pdf");
        }
    }
}