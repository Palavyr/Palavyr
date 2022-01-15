using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Requests.Registration;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Services.AccountServices
{
    public interface IAccountSetupService
    {
        Task<Credentials> CreateNewAccountViaDefaultAsync(AccountDetails newAccountDetails, CancellationToken cancellationToken);
    }
}