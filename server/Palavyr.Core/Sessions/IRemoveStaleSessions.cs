using System.Threading.Tasks;

namespace Palavyr.Core.Sessions
{
    public interface IRemoveStaleSessions
    {
        Task CleanSessionDb(string accountId);
    }
}