using System.Threading;
using System.Threading.Tasks;

namespace Palavyr.Core.Repositories.Delete
{
    public interface IAccountDeleter : IAccountRepository
    {
        Task DeleteAccount(string accountId, CancellationToken cancellationToken);
        Task DeleteAccountRecord(string accountId, CancellationToken cancellationToken);
        void DeleteEmailVerifications(string accountId);
        void DeleteSessionsByAccount(string accountId);
        void DeleteSubscriptionsByAccount(string accountId);
    }
}