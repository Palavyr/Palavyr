using System.IO;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.CloudKeyResolvers
{
    public interface ICloudKeyResolver
    {
        string ResolveFileAssetKey(FileName fileName);
        string ResolveResponsePdfKey(FileName fileName);
        string ResolveResponsePdfPreviewKey(FileName fileName);
    }

    public class CloudKeyResolver : ICloudKeyResolver
    {
        private readonly IAccountIdTransport accountIdTransport;

        public CloudKeyResolver(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public string ResolveFileAssetKey(FileName fileName)
        {
            // bucket
            // accountId / file-assets / fileId.Extension
            return Path.Combine(accountIdTransport.AccountId, "file-assets", string.Join(".", fileName.FileId, fileName.Extension));
        }

        public string ResolveResponsePdfKey(FileName fileName)
        {
            // bucket
            // accountId / response-pdfs / fileId.Extension
            return Path.Combine(accountIdTransport.AccountId, "response-pdfs", string.Join(".", fileName.FileId, fileName.Extension));
        }

        public string ResolveResponsePdfPreviewKey(FileName fileName)
        {
            // bucket
            // accountId / response-preview-pdfs / fileId.Extension
            return Path.Combine(accountIdTransport.AccountId, "response-preview-pdfs", string.Join(".", fileName.FileId, fileName.Extension));
        }
    }
}