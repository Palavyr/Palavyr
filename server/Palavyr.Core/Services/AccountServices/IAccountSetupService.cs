using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Services.AccountServices
{
    public interface IAccountSetupService
    {
        Task<Credentials> CreateNewAccount(string emailAddress, string password, CancellationToken cancellationToken);
    }
}