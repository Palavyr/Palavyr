using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Services.CloudKeyResolvers
{
    public interface IResponsePdfPreviewKeyResolver : IResolveCloudKeys
    {
    }

    public class ResponsePdfPreviewKeyResolver : IResponsePdfPreviewKeyResolver
    {
        private readonly ICloudCompatibleKeyResolver cloudKeyResolver;

        public ResponsePdfPreviewKeyResolver(ICloudCompatibleKeyResolver cloudKeyResolver)
        {
            this.cloudKeyResolver = cloudKeyResolver;
        }

        public string Resolve(FileName fileName)
        {
            return cloudKeyResolver.ResolveResponsePdfPreviewKey(fileName);
        }
    }
}