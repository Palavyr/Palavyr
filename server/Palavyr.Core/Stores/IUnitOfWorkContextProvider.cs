using System.Threading.Tasks;
using Palavyr.Core.Data;

namespace Palavyr.Core.Stores
{
    public interface IUnitOfWorkContextProvider
    {
        AppDataContexts Data { get; set; }

        Task DangerousCommitAllContexts();
        Task CloseUnitOfWork();
        Task DisposeContexts();
    }
}