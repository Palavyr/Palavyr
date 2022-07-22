using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Component.ComponentTestBase;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Stores;

namespace Palavyr.Component.Mocks
{
    public class MockEmailVerificationService : IEmailVerificationService
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly IEntityStore<AccountEmailVerification> emailVerificationStore;

        public MockEmailVerificationService(IEntityStore<Account> accountStore, IEntityStore<AccountEmailVerification> emailVerificationStore)
        {
            this.accountStore = accountStore;
            this.emailVerificationStore = emailVerificationStore;
        }

        public async Task<bool> ConfirmEmailAddressAsync(string authToken, CancellationToken cancellationToken)
        {
            var verificationRecord = await emailVerificationStore
                .DangerousRawQuery()
                .SingleOrDefaultAsync(x => x.AuthenticationToken == ComponentTest.ConfirmationToken);
            var account = await accountStore.Get(verificationRecord.AccountId, s => s.AccountId);
            account.Active = true;

            return true;
        }

        public async Task<bool> SendConfirmationTokenEmail(string emailAddress, CancellationToken cancellationToken)
        {
            var confirmationToken = ComponentTest.ConfirmationToken;
            await emailVerificationStore.Create(AccountEmailVerification.CreateNew(confirmationToken, emailAddress, emailVerificationStore.AccountId));
            return true;
        }
    }
}