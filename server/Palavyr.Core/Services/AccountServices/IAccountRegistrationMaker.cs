using System.Threading;
using System.Threading.Tasks;

namespace Palavyr.Core.Services.AccountServices
{
    public interface IAccountRegistrationMaker
    {
        Task<bool> TryRegisterAccountAndSendEmailVerificationToken(string accountId, string apiKey, string emailAddress, string introId, CancellationToken cancellationToken);
    }
}