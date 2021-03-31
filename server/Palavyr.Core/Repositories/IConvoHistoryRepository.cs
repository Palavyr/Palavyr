using System.Threading.Tasks;

namespace Palavyr.Core.Repositories
{
    public interface IConvoHistoryRepository
    {
        Task CommitChangesAsync();
    }
}