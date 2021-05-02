using System.IO;
using Palavyr.Core.Common.ExtensionMethods.PathExtensions;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3KeyResolver
    {
        string ResolveAttachmentKey(string accountId, string areaId, string safeFileName);
        string ResolvePreviewKey(string accountId, string safeFileName);
        string ResolveLogoKey(string account, string safeFileName);
    }

    public class S3KeyResolver : IS3KeyResolver
    {
        public string ResolveAttachmentKey(string accountId, string areaId, string safeFileName)
        {
            return Path.Combine(accountId, "AreaData", areaId, "Attachments", safeFileName + ".pdf").ConvertToUnix();
        }

        public string ResolvePreviewKey(string accountId, string safeFileName)
        {
            return Path.Combine(accountId, "previews", safeFileName + ".pdf").ConvertToUnix();
        }

        public string ResolveLogoKey(string account, string safeFileName)
        {
            return Path.Combine(account, "logos", safeFileName + ".pdf").ConvertToUnix();
        }
    }
}