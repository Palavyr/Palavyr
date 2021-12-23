using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    public class ResendConfirmationTokenController : PalavyrBaseController
    {
        private readonly IEmailVerificationService emailVerificationService;
        private readonly AccountsContext accountsContext;
        private readonly IHoldAnAccountId accountIdHolder;

        public ResendConfirmationTokenController(
            IEmailVerificationService emailVerificationService,
            AccountsContext accountsContext,
            IHoldAnAccountId accountIdHolder
        )
        {
            this.emailVerificationService = emailVerificationService;
            this.accountsContext = accountsContext;
            this.accountIdHolder = accountIdHolder;
        }


        [HttpPost("account/confirmation/token/resend")]
        public async Task<bool> Post(
            [FromBody] EmailVerificationRequest emailRequest,
            CancellationToken cancellationToken
        )
        {
            // delete any old records
            var maybeCurrentRecord = accountsContext.EmailVerifications
                .SingleOrDefault(x => x.EmailAddress == emailRequest.EmailAddress && x.AccountId == accountIdHolder.AccountId);
            if (maybeCurrentRecord != null)
            {
                accountsContext.EmailVerifications.Remove(maybeCurrentRecord);
                await accountsContext.SaveChangesAsync(cancellationToken);
            }

            // resend
            var result = await emailVerificationService.SendConfirmationTokenEmail(emailRequest.EmailAddress, cancellationToken);
            return result;
        }
    }
}