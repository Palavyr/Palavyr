using System.Threading;
using System.Threading.Tasks;

namespace Palavyr.Core.Data
{
    public interface IDataContext
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken);
        void BeginTransaction();
        Task FinalizeAsync(CancellationToken cancellationToken);
    }
}