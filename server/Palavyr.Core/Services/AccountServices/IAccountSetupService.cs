using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.Core.Services.AccountServices
{
    public interface IAccountSetupService
    {
        Task<Credentials> CreateNewAccountViaDefaultAsync(string emailAddress, string password, CancellationToken cancellationToken);
    }
}