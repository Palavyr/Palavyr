
using System.Threading.Tasks;

namespace Palavyr.Core.Services.FileAssetServices.FileAssetLinkers
{
    public interface IFileAssetLinker<TLinker> where TLinker : class
    {
        Task Link(string fileId, string? targetId = null);
        Task Unlink(string? fileId, string? targetId = null);
    
    }
}