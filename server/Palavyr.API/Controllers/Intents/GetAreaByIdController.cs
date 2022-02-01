using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.EmailService.Verification;

namespace Palavyr.API.Controllers.Intents
{
    // TODO: Wtf is going on in this controller. This must be really old.
    // I Don't think this is even being used any more by the client.
    
    [Obsolete]
    [Authorize]
    public class GetAreaByIdController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IEmailVerificationStatus emailVerificationStatus;
        private ILogger<GetAreaByIdController> logger;

        public GetAreaByIdController(
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository,
            IEmailVerificationStatus emailVerificationStatus,
            ILogger<GetAreaByIdController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
        }

        [Obsolete]
        ///https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html
        /// The verification status of an email address is "Pending" until the email address owner clicks the link within the verification email that Amazon SES sent to that address. If the email address owner clicks the link within 24 hours, the verification status of the email address changes to "Success". If the link is not clicked within 24 hours, the verification status changes to "Failed." In that case, if you still want to verify the email address, you must restart the verification process from the beginning.
        [HttpGet("areas/{areaId}")]
        public async Task<Area> Get(
            string areaId,
            CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(areaId);

            if (string.IsNullOrWhiteSpace(area.AreaSpecificEmail))
            {
                var account = await accountRepository.GetAccount();
                area.AreaSpecificEmail = account.EmailAddress;
            }

            var (found, status) = await emailVerificationStatus.RequestEmailVerificationStatus(area.AreaSpecificEmail);
            if (!found)
            {
                throw new Exception("Default email not found. Account is corrupted.");
            }

            var statusResponse = emailVerificationStatus.HandleFoundEmail(status, area.AreaSpecificEmail);

            area.EmailIsVerified = statusResponse.IsVerified();
            area.AwaitingVerification = statusResponse.IsPending();

            if (area.UseAreaFallbackEmail == null) // code smell
            {
                area.UseAreaFallbackEmail = false;
            }

            await configurationRepository.CommitChangesAsync();
            return area;
        }
    }
}