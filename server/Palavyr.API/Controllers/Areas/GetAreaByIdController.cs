using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.EmailService.Verification;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]

    public class GetAreaByIdController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;
        private readonly EmailVerificationStatus emailVerificationStatus;
        private ILogger<GetAreaByIdController> logger;

        public GetAreaByIdController(
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository,
            EmailVerificationStatus emailVerificationStatus,
            ILogger<GetAreaByIdController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
            this.emailVerificationStatus = emailVerificationStatus;
            this.logger = logger;
        }

        ///https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html
        /// The verification status of an email address is "Pending" until the email address owner clicks the link within the verification email that Amazon SES sent to that address. If the email address owner clicks the link within 24 hours, the verification status of the email address changes to "Success". If the link is not clicked within 24 hours, the verification status changes to "Failed." In that case, if you still want to verify the email address, you must restart the verification process from the beginning.
        [HttpGet("areas/{areaId}")]
        public async Task<Area> Get([FromHeader] string accountId, string areaId)
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);

            if (string.IsNullOrWhiteSpace(area.AreaSpecificEmail))
            {
                var account = await accountRepository.GetAccount(accountId);
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