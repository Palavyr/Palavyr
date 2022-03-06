using System.Threading.Tasks;

namespace Palavyr.Core.Services.FileAssetServices.FileAssetLinkers
{
    public interface IFileAssetLinker<TLinker> where TLinker : class
    {
        Task LinkToIntent(string fileId, string intentId);
        Task LinkToNode(string fileId, string nodeId);
        Task LinkToAccount(string fileId);

        Task UnLinkFromIntent(string fileId, string intentId);
        Task UnLinkFromNode(string fileId, string nodeId);
        Task UnlinkFromAccount(string fileId);
    }
}