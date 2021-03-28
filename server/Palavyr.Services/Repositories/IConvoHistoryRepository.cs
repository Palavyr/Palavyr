using System.Threading.Tasks;

namespace Palavyr.Services.Repositories
{
    public interface IConvoHistoryRepository
    {
        Task CommitChangesAsync();
    }
}