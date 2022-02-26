using System.IO;
using Palavyr.Core.Common.ExtensionMethods.PathExtensions;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3KeyResolver
    {
        string ResolveAttachmentKey(string areaId, string safeFileName);
        string ResolvePreviewKey(string safeFileName);
        string ResolveLogoKey(string safeFileName, string fileExtension);
        string ResolveResponsePdfKey(string safeFileName);
        string ResolveImageKey(string safeName);
    }

    public class S3KeyResolver : IS3KeyResolver
    {
        private readonly IAccountIdTransport accountIdTransport;

        public S3KeyResolver(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public string ResolveAttachmentKey(string areaId, string safeFileName)
        {
            return Path.Combine(accountIdTransport.AccountId, "AreaData", areaId, "Attachments", safeFileName + ".pdf").ConvertToUnix();
        }

        public string ResolvePreviewKey(string safeFileName)
        {
            return Path.Combine(accountIdTransport.AccountId, "Previews", safeFileName + ".pdf").ConvertToUnix();
        }

        public string ResolveLogoKey(string safeFileName, string fileExtension)
        {
            return Path.Combine(accountIdTransport.AccountId, "Logos", safeFileName + fileExtension).ConvertToUnix();
        }

        public string ResolveResponsePdfKey(string safeFileName)
        {
            return Path.Combine(accountIdTransport.AccountId, "Responses", safeFileName + ".pdf").ConvertToUnix();
        }

        public string ResolveImageKey(string safeName) // safename includes extension
        {
            return Path.Combine(accountIdTransport.AccountId, "Images", safeName).ConvertToUnix();
        }
    }
}