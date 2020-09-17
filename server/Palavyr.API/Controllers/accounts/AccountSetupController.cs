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
        /// Creates a new account table record and  a new database
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<StatusCodeResult> CreateNewAccount([FromBody] AccountDetails newAccount)
        {
            var newAccountId = GetNewAccountId();
            var randomUserId = Guid.NewGuid().ToString(); // TODO: decide how to make use of the username concept here
            var newApiKey = Guid.NewGuid().ToString();

            var account = AccountContext.Accounts.SingleOrDefault(row => row.EmailAddress == newAccount.EmailAddress);
            if (account != null)
            {
                return new ConflictResult();
            }
            
            var newAccountRecord = UserAccount.CreateAccount(
                randomUserId,
                newAccount.EmailAddress,
                PasswordHashing.CreateHashedPassword(newAccount.Password),
                newAccountId,
                newApiKey);

            await AccountContext.Accounts.AddAsync(newAccountRecord);

            var newSubscription = Subscription.CreateNew(newAccountId, newApiKey, SubscriptionConstants.DefaultNumAreas);
            await AccountContext.Subscriptions.AddAsync(newSubscription);
            
            var data = new AccountSeedData(newAccountId);
            await DashContext.Areas.AddRangeAsync(data.Areas);
            await DashContext.Groups.AddRangeAsync(data.Groups);

            await DashContext.SaveChangesAsync();

            var confirmationToken = Guid.NewGuid().ToString();
            var fromAddress = "gradie.machine.learning@gmail.com";
            var subject = "Welcome to Palavyr - Email Verification";
            await AccountContext.EmailVerifications.AddAsync(EmailVerification.CreateNew(confirmationToken, newAccount.EmailAddress, newAccountId));
            await AccountContext.SaveChangesAsync();
            
            var htmlBody = EmailConfirmationHTML.GetConfirmationEmailBody(newAccount, confirmationToken);
            var textBody = EmailConfirmationHTML.GetConfirmationEmailBodyText(newAccount, confirmationToken);

            var ok = await Client.SendEmail(fromAddress, newAccount.EmailAddress, subject, htmlBody, textBody);

            return ok ? (StatusCodeResult) new OkResult() : new NotFoundResult();
        }

        public string GetNewAccountId()
        {
            var ids = Guid.NewGuid().ToString().Split('-').Take(2).ToList();
            return string.Join('-', ids);
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