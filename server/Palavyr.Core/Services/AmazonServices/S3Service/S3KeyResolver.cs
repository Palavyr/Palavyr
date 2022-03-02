#nullable enable
using System.IO;
using Palavyr.Core.Common.ExtensionMethods.PathExtensions;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.LogoServices;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.AmazonServices.S3Service
{
    public interface IS3KeyResolver
    {
        string ResolveAttachmentKey(string areaId, string safeFileName);
        string ResolvePreviewKey(string safeFileName);
        string ResolveLogoKey(string safeFileName, string fileExtension);
        string ResolveResponsePdfKey(string safeFileName);
        string ResolveImageKey(string safeNameWithExtension);
    }


    public interface IResolveS3Key<TAttachmentKey>
    {
        string Resolve(FileName fileName, string? intentId = null);
    }

    public class AttachmentKey : IResolveS3Key<AttachmentKey>
    {
        private readonly IAccountIdTransport accountIdTransport;

        public AttachmentKey(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public string Resolve(FileName fileName, string? intentId)
        {
            if (intentId == null) throw new DomainException("Intent Id required for resolving attachment save location.");

            var safeFileName = fileName.FileStem;
            var suffix = fileName.HasSuffix ? fileName.Suffix : ".pdf";
            return Path.Combine(accountIdTransport.AccountId, "AreaData", intentId, "Attachments", Path.Join(".", safeFileName, suffix)).ConvertToUnix();
        }
    }

    public class LogoKey : IResolveS3Key<LogoKey>
    {
        private readonly IAccountIdTransport accountIdTransport;

        public LogoKey(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public string Resolve(FileName fileName, string? intentId)
        {
            return Path.Combine(accountIdTransport.AccountId, "Logos", Path.Join(".", fileName.FileStem, fileName.Suffix)).ConvertToUnix();
        }
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

        public string ResolveLogoKey(FileName fileName)
        {
            return ResolveLogoKey(fileName.FileStem, fileName.Suffix);
        }
        
        public string ResolveLogoKey(string safeFileName, string fileExtension)
        {
            return Path.Combine(accountIdTransport.AccountId, "Logos", safeFileName + fileExtension).ConvertToUnix();
        }

        public string ResolveResponsePdfKey(string safeFileName)
        {
            return Path.Combine(accountIdTransport.AccountId, "Responses", safeFileName + ".pdf").ConvertToUnix();
        }

        public string ResolveImageKey(string safeNameWithExtension) // safename includes extension
        {
            return Path.Combine(accountIdTransport.AccountId, "Images", safeNameWithExtension).ConvertToUnix();
        }
    }
}