using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.AccountServices;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    public class ResendConfirmationTokenController : PalavyrBaseController
    {
        private readonly IEmailVerificationService emailVerificationService;
        private readonly AccountsContext accountsContext;

        public ResendConfirmationTokenController(
            IEmailVerificationService emailVerificationService,
            AccountsContext accountsContext
        )
        {
            this.emailVerificationService = emailVerificationService;
            this.accountsContext = accountsContext;
        }


        [HttpPost("account/confirmation/token/resend")]
        public async Task<bool> Post(
            [FromHeader] string accountId,
            [FromBody] EmailVerificationRequest emailRequest,
            CancellationToken cancellationToken
        )
        {
            // delete any old records
            var maybeCurrentRecord = accountsContext.EmailVerifications
                .SingleOrDefault(x => x.EmailAddress == emailRequest.EmailAddress && x.AccountId == accountId);
            if (maybeCurrentRecord != null)
            {
                accountsContext.EmailVerifications.Remove(maybeCurrentRecord);
                await accountsContext.SaveChangesAsync(cancellationToken);
            }

            // resend
            var result = await emailVerificationService.SendConfirmationTokenEmail(emailRequest.EmailAddress, accountId);
            return result;
        }
    }
}