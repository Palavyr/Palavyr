using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Stores;

namespace Palavyr.API.Controllers.Intents
{
    // TODO: Wtf is going on in this controller. This must be really old.
    // I Don't think this is even being used any more by the client.

    [Obsolete]
    public class GetIntentByIdController : PalavyrBaseController
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly IEntityStore<Account> accountStore;
        private readonly IEmailVerificationStatus emailVerificationStatus;
        private ILogger<GetIntentByIdController> logger;

        public GetIntentByIdController(
            IEntityStore<Intent> intentStore,
            IEntityStore<Account> accountStore,
            IEmailVerificationStatus emailVerificationStatus,
            ILogger<GetIntentByIdController> logger
        )
        {
            this.intentStore = intentStore;
            this.accountStore = accountStore;
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
        }

        [Obsolete]
        ///https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html
        /// The verification status of an email address is "Pending" until the email address owner clicks the link within the verification email that Amazon SES sent to that address. If the email address owner clicks the link within 24 hours, the verification status of the email address changes to "Success". If the link is not clicked within 24 hours, the verification status changes to "Failed." In that case, if you still want to verify the email address, you must restart the verification process from the beginning.
        [HttpGet("areas/{intentId}")]
        public async Task<Intent> Get(
            string intentId,
            CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(intentId, s => s.IntentId);

            if (string.IsNullOrWhiteSpace(intent.IntentSpecificEmail))
            {
                var account = await accountStore.Get(accountStore.AccountId, s => s.AccountId);
                intent.IntentSpecificEmail = account.EmailAddress;
            }

            var (found, status) = await emailVerificationStatus.RequestEmailVerificationStatus(intent.IntentSpecificEmail);
            if (!found)
            {
                throw new Exception("Default email not found. Account is corrupted.");
            }

            var statusResponse = emailVerificationStatus.HandleFoundEmail(status, intent.IntentSpecificEmail);

            intent.EmailIsVerified = statusResponse.IsVerified();
            intent.AwaitingVerification = statusResponse.IsPending();

            if (intent.UseIntentFallbackEmail == null) // code smell
            {
                intent.UseIntentFallbackEmail = false;
            }

            return intent;
        }
    }
}