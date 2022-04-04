using System.IO;
using Palavyr.Core.Common.ExtensionMethods.PathExtensions;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.CloudKeyResolvers
{
    public interface ICloudCompatibleKeyResolver
    {
        string ResolveFileAssetKey(FileName fileName);
        string ResolveResponsePdfKey(FileName fileName);
        string ResolveResponsePdfPreviewKey(FileName fileName);
    }

    public class CloudCompatibleKeyResolver : ICloudCompatibleKeyResolver
    {
        private readonly IAccountIdTransport accountIdTransport;

        public CloudCompatibleKeyResolver(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public string ResolveFileAssetKey(FileName fileName)
        {
            // bucket
            // accountId / file-assets / fileId.Extension
            return UnixCombine(accountIdTransport.AccountId, "file-assets", fileName.ToString());
        }

        public string ResolveResponsePdfKey(FileName fileName)
        {
            // bucket
            // accountId / response-pdfs / fileId.Extension
            return UnixCombine(accountIdTransport.AccountId, "response-pdfs", fileName.ToString());
        }

        public string ResolveResponsePdfPreviewKey(FileName fileName)
        {
            // bucket
            // accountId / response-preview-pdfs / fileId.Extension
            return UnixCombine(accountIdTransport.AccountId, "response-preview-pdfs", fileName.ToString());
        }

        public static string UnixCombine(params string[] args)
        {
            return Path.Combine(args).ConvertToUnix();
        }
    }
}