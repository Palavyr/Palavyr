using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Services.CloudKeyResolvers
{
    public interface IFileAssetKeyResolver : IResolveCloudKeys
    {
    }

    public class FileAssetKeyResolver : IFileAssetKeyResolver
    {
        private readonly ICloudKeyResolver cloudKeyResolver;

        public FileAssetKeyResolver(ICloudKeyResolver cloudKeyResolver)
        {
            this.cloudKeyResolver = cloudKeyResolver;
        }

        public string Resolve(FileName fileName)
        {
            return cloudKeyResolver.ResolveFileAssetKey(fileName);
        }
    }
}