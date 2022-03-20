using Palavyr.Core.Services.FileAssetServices;

namespace Palavyr.Core.Services.CloudKeyResolvers
{
    public interface IResolveCloudKeys
    {
        string Resolve(FileName fileName);
    }
}