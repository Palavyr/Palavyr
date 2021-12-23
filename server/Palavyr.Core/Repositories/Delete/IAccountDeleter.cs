using System.Threading.Tasks;

namespace Palavyr.Core.Repositories.Delete
{
    public interface IAccountDeleter : IAccountRepository
    {
        Task DeleteAccount();
        Task DeleteAccountRecord();
        void DeleteEmailVerifications();
        void DeleteSessionsByAccount();
        void DeleteSubscriptionsByAccount();
    }
}