using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Services.CloudKeyResolvers
{
    public interface IPdfResponseKeyResolver : IResolveCloudKeys
    {
    }

    public class PdfResponseKeyResolver : IPdfResponseKeyResolver
    {
        private readonly ICloudCompatibleKeyResolver cloudKeyResolver;

        public PdfResponseKeyResolver(ICloudCompatibleKeyResolver cloudKeyResolver)
        {
            this.cloudKeyResolver = cloudKeyResolver;
        }

        public string Resolve(FileName fileName)
        {
            return cloudKeyResolver.ResolveResponsePdfKey(fileName);
        }
    }
}