using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Services.ImageServices
{
    public interface IImageRemover
    {
        Task<FileLink[]> RemoveImages(string[] ids, string accountId, CancellationToken cancellationToken);
    }
}