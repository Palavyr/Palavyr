using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.IntegrationTests.Mocks
{
    public class MockEmailVerificationService : IEmailVerificationService
    {
        public Task<bool> ConfirmEmailAddressAsync(string authToken, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SendConfirmationTokenEmail(string emailAddress, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}