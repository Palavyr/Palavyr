using System.Threading.Tasks;
using Palavyr.Core.Data;

namespace Palavyr.Core.Repositories
{
    public interface IUnitOfWorkContextProvider
    {
        AccountsContext AccountsContext();
        ConvoContext ConvoContext();
        DashContext ConfigurationContext();
        Task CloseUnitOfWork();
    }
}