using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using EmailService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.API.controllers.accounts.seedData;
using Palavyr.Common.Constants;
using Palavyr.Common.uniqueIdentifiers;
using Server.Domain.AccountDB;
using Server.Domain.Accounts;

namespace Palavyr.API.controllers.accounts.newAccount
{
    [Route("api/account")]
    [ApiController]
    public class AccountSetup : BaseController
    {
        private static ILogger<AccountSetup> _logger;
        private SESEmail Client { get; set; } // Startup.cs handles finding credentials from appsettings.json through GetAWSOptions

        public AccountSetup(
            ILogger<AccountSetup> logger,
            IAmazonSimpleEmailService SES,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
            Client = new SESEmail(logger, SES);
            _logger = logger;
        }
        
        /// <summary>
        /// Creates a new account table record and data
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<StatusCodeResult> CreateNewAccount([FromBody] AccountDetails newAccountRequest)
        {
            _logger.LogDebug("Creating a new account");
            var newAccountId = NewAccountUtils.GetNewAccountId();
            var newUserId = Guid.NewGuid().ToString(); // TODO: decide how to make use of the username concept here
            var newApiKey = Guid.NewGuid().ToString();

            // confirm account doesn't already exist
            var account = AccountContext.Accounts.SingleOrDefault(row => row.EmailAddress == newAccountRequest.EmailAddress);
            if (account != null)
            {
                _logger.LogDebug($"Account for email address {newAccountRequest.EmailAddress} already exists");
                return new ConflictResult();
            }
            
            // Add the new account
            var newAccount = UserAccount.CreateAccount(
                newUserId,
                newAccountRequest.EmailAddress,
                PasswordHashing.CreateHashedPassword(newAccountRequest.Password),
                newAccountId,
                newApiKey);
            
            _logger.LogDebug("Adding new account...");
            await AccountContext.Accounts.AddAsync(newAccount);

            // Add the default subscription (free with 2 areas)
            _logger.LogDebug($"Add default subscription for {newAccountId}");
            var newSubscription = Subscription.CreateNew(newAccountId, newApiKey, SubscriptionConstants.DefaultNumAreas);
            await AccountContext.Subscriptions.AddAsync(newSubscription);
            
            // install seed Data
            _logger.LogDebug("Install new account seed data.");
            var seeData = new SeedData(newAccountId);
            await DashContext.Areas.AddRangeAsync(seeData.Areas);
            await DashContext.Groups.AddRangeAsync(seeData.Groups);
            await DashContext.Areas.AddRangeAsync(seeData.Areas);
            await DashContext.Groups.AddRangeAsync(seeData.Groups);
            await DashContext.WidgetPreferences.AddAsync(seeData.WidgetPreference);
            await DashContext.SelectOneFlats.AddRangeAsync(seeData.DefaultDynamicTables);
            await DashContext.DynamicTableMetas.AddRangeAsync(seeData.DefaultDynamicTableMetas);
            await DashContext.SaveChangesAsync();

            // prepare the account confirmation email
            _logger.LogDebug("Provide an account setup confirmation token");
            var confirmationToken = Guid.NewGuid().ToString().Split("-")[0];
            await AccountContext.EmailVerifications.AddAsync(EmailVerification.CreateNew(confirmationToken, newAccountRequest.EmailAddress, newAccountId));
            await AccountContext.SaveChangesAsync();

            // send the confirmation email - handle a bounceback if the email address is not real
            const string fromAddress = "gradie.machine.learning@gmail.com"; // TODO: Replace with company email asap
            const string subject = "Welcome to Palavyr - Email Verification";
            _logger.LogDebug($"Sending emails from {fromAddress}");
            var htmlBody = EmailConfirmationHTML.GetConfirmationEmailBody(newAccountRequest, confirmationToken);
            var textBody = EmailConfirmationHTML.GetConfirmationEmailBodyText(newAccountRequest, confirmationToken);
            var ok = await Client.SendEmail(fromAddress, newAccountRequest.EmailAddress, subject, htmlBody, textBody);
            _logger.LogDebug("Send Email result was " + (ok ? "OK" : "FAIL"));
            return ok ? (StatusCodeResult) new OkResult() : new NotFoundResult();
        }
        

        [HttpPost("confirmation/{authToken}/action/setup")]
        public bool ConfirmEmailAddress(string authToken)
        {
            var emailVerification = AccountContext.EmailVerifications.SingleOrDefault(row => row.AuthenticationToken == authToken.Trim());
            if (emailVerification == null)
                return false;

            var accountId = emailVerification.AccountId;
            var account = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            if (account == null)
                return false;

            account.Active = true;
            AccountContext.EmailVerifications.Remove(emailVerification);
            AccountContext.SaveChanges();

            return true;
        }

        [HttpGet("isActive")]
        public bool CheckIsActive([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            var isActive = account.Active;
            return isActive;
        }
        
    }
}