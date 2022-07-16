using System.Threading.Tasks;
using Palavyr.Core.Data;

namespace Palavyr.Core.Stores
{
    public interface IUnitOfWorkContextProvider
    {
        AppDataContexts AppDataContexts();
        Task DangerousCommitAllContexts();
        Task CloseUnitOfWork();
        Task DisposeContexts();
    }
}