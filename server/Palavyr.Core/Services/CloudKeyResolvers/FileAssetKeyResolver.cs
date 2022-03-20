using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Services.CloudKeyResolvers
{
    public interface IFileAssetKeyResolver : IResolveCloudKeys
    {
    }

    public class FileAssetKeyResolver : IFileAssetKeyResolver
    {
        private readonly ICloudCompatibleKeyResolver cloudCompatibleKeyResolver;

        public FileAssetKeyResolver(ICloudCompatibleKeyResolver cloudCompatibleKeyResolver)
        {
            this.cloudCompatibleKeyResolver = cloudCompatibleKeyResolver;
        }

        public string Resolve(FileName fileName)
        {
            return cloudCompatibleKeyResolver.ResolveFileAssetKey(fileName);
        }
    }
}