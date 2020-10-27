using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using EmailService;
using EmailService.verification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.ResponseTypes;
using Server.Domain.AccountDB;

namespace Palavyr.API.controllers.accounts.newAccount
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountSetup : BaseController
    {
        private static ILogger<AccountSetup> _logger;
        private readonly IAccountSetupService _setupService;
        private readonly IEmailVerificationService _emailVerificationService;
        private SESEmail Client { get; set; }
        private SenderVerification Verifier { get; set; }

        public AccountSetup(
            IEmailVerificationService emailVerificationService,
            IAccountSetupService setupService,
            ILogger<AccountSetup> logger,
            IAmazonSimpleEmailService SESClient,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
            Client = new SESEmail(logger, SESClient);
            Verifier = new SenderVerification(logger, SESClient);
            _logger = logger;
            _setupService = setupService;
            _emailVerificationService = emailVerificationService;
        }

        /// <summary>
        /// Creates a new account table record and data
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<Credentials> CreateNewAccount([FromBody] AccountDetails newAccountDetails)
        {
            return await _setupService.CreateNewAccountViaDefaultAsync(newAccountDetails);
        }

        [AllowAnonymous]
        [HttpPost("create/google")]
        public async Task<Credentials> CreateNewAccountGoogle([FromBody] GoogleRegistrationDetails registrationDetails)
        {
            return await _setupService.CreateNewAccountViaGoogleAsync(registrationDetails);
        }


        [HttpPost("confirmation/{authToken}/action/setup")]
        public async Task<bool> ConfirmEmailAddress(string authToken)
        {
            return await _emailVerificationService.ConfirmEmailAddressAsync(authToken);

        }

        [HttpGet("isActive")]
        public bool CheckIsActive([FromHeader] string accountId)
        {
            _logger.LogDebug("Activation controller hit! Again!");
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            var isActive = account.Active;
            return isActive;
        }
    }
}