using System.IO;
using Palavyr.Core.Common.ExtensionMethods.PathExtensions;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3KeyResolver
    {
        string ResolveAttachmentKey(string accountId, string areaId, string safeFileName);
        string ResolvePreviewKey(string accountId, string safeFileName);
        string ResolveLogoKey(string account, string safeFileName, string fileExtension);
        string ResolveResponsePdfKey(string account, string safeFileName);
        string ResolveImageKey(string account, string safeName);
    }

    public class S3KeyResolver : IS3KeyResolver
    {
        public string ResolveAttachmentKey(string accountId, string areaId, string safeFileName)
        {
            return Path.Combine(accountId, "AreaData", areaId, "Attachments", safeFileName + ".pdf").ConvertToUnix();
        }

        public string ResolvePreviewKey(string accountId, string safeFileName)
        {
            return Path.Combine(accountId, "Previews", safeFileName + ".pdf").ConvertToUnix();
        }

        public string ResolveLogoKey(string account, string safeFileName, string fileExtension)
        {
            return Path.Combine(account, "Logos", safeFileName + fileExtension).ConvertToUnix();
        }

        public string ResolveResponsePdfKey(string account, string safeFileName)
        {
            return Path.Combine(account, "Responses", safeFileName + ".pdf").ConvertToUnix();
        }

        public string ResolveImageKey(string account, string safeName) // safename includes extension
        {
            return Path.Combine(account, "Images", safeName).ConvertToUnix();
        }
    }
}