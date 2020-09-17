using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.AccountDB;
using Microsoft.Extensions.Logging;
using EmailService;
using Microsoft.Extensions.Hosting;
using Palavyr.Common.Constants;


namespace Palavyr.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountSetup : BaseController
    {
        // private static ILogger<Authentication> _logger;
        private SESEmail Client { get; set; } // Startup.cs handles finding credentials from appsettings.json through GetAWSOptions

        public AccountSetup(
            
            IAmazonSimpleEmailService SES,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
            Client = new SESEmail(SES);
        }


        /// <summary>
        /// Creates a new account table record and data
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<StatusCodeResult> CreateNewAccount([FromBody] AccountDetails newAccountRequest)
        {
            var newAccountId = NewAccountUtils.GetNewAccountId();
            var newUserId = Guid.NewGuid().ToString(); // TODO: decide how to make use of the username concept here
            var newApiKey = Guid.NewGuid().ToString();

            // confirm account doesn't already exist
            var account = AccountContext.Accounts.SingleOrDefault(row => row.EmailAddress == newAccountRequest.EmailAddress);
            if (account != null)
            {
                return new ConflictResult();
            }
            
            // Add the new account
            var newAccount = UserAccount.CreateAccount(
                newUserId,
                newAccountRequest.EmailAddress,
                newAccountRequest.Password,
                newAccountId,
                newApiKey);
            await AccountContext.Accounts.AddAsync(newAccount);

            // Add the default subscription (free with 2 areas)
            var newSubscription = Subscription.CreateNew(newAccountId, newApiKey, SubscriptionConstants.DefaultNumAreas);
            await AccountContext.Subscriptions.AddAsync(newSubscription);
            
            // install seed Data
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
            var confirmationToken = Guid.NewGuid().ToString();
            await AccountContext.EmailVerifications.AddAsync(EmailVerification.CreateNew(confirmationToken, newAccountRequest.EmailAddress, newAccountId));
            await AccountContext.SaveChangesAsync();

            // send the confirmation email - handle a bounceback if the email address is not real
            const string fromAddress = "gradie.machine.learning@gmail.com"; // TODO: Replace with company email asap
            const string subject = "Welcome to Palavyr - Email Verification";
            var htmlBody = EmailConfirmationHTML.GetConfirmationEmailBody(newAccountRequest, confirmationToken);
            var textBody = EmailConfirmationHTML.GetConfirmationEmailBodyText(newAccountRequest, confirmationToken);
            var ok = await Client.SendEmail(fromAddress, newAccountRequest.EmailAddress, subject, htmlBody, textBody);

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