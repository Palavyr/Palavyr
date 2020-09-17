using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.ReceiverTypes;


namespace Palavyr.API.Controllers
{
    [Route("api/account/settings")]
    [ApiController]
    public class AccountSettings : BaseController
    {
        public AccountSettings(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext,
            IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
        }

        [HttpPut("update/password")]
        public bool UpdatePassword([FromHeader] string accountId, LoginCredentials login)
        {
            var account = AccountContext
                .Accounts
                .Single(row => row.AccountId == accountId);

            var oldHashedPassword = login.OldPassword;
            if (oldHashedPassword != login.Password)
            {
                return false;
            }

            account.Password = login.Password;
            AccountContext.SaveChanges();
            return true;
        }

        [HttpPut("update/email")]
        public StatusCodeResult UpdateEmail([FromHeader] string accountId, LoginCredentials login)
        {
            var account = AccountContext
                .Accounts
                .Single(row => row.AccountId == accountId);
            account.EmailAddress = login.EmailAddress;
            AccountContext.SaveChanges();
            return new OkResult();
        }

        [HttpPut("update/companyname")]
        public StatusCodeResult UpdateCompanyName([FromHeader] string accountId, LoginCredentials login)
        {
            var account = AccountContext
                .Accounts
                .Single(row => row.AccountId == accountId);
            account.CompanyName = login.CompanyName;
            AccountContext.SaveChanges();
            return new OkResult();
        }

        [HttpPut("update/username")]
        public StatusCodeResult UpdateUserName([FromHeader] string accountId, LoginCredentials login)
        {
            var allUserNames = AccountContext.Accounts.ToList().Select(x => x.UserName);
            if (allUserNames.Contains(login.Username))
            {
                return new BadRequestResult();
            }

            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            account.UserName = login.Username;
            AccountContext.SaveChanges();
            return new OkResult();
        }

        [HttpPut("update/phonenumber")]
        public StatusCodeResult UpdatePhoneNumber([FromHeader] string accountId, LoginCredentials login)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            account.PhoneNumber = login.PhoneNumber;
            AccountContext.SaveChanges();
            return new OkResult();
        }

        [HttpGet("companyname")]
        public string GetCompanyName([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.CompanyName ?? "";
        }

        [HttpGet("email")]
        public string GetEmail([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.EmailAddress ?? "";
        }

        [HttpGet("username")]
        public string GetUserName([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.UserName ?? "";
        }

        [HttpGet("phonenumber")]
        public string GetPhoneNumber([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.PhoneNumber ?? "";
        }

        [HttpGet("apikey")]
        public string GetApiKey([FromHeader] string accountId)
        {
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            return account.ApiKey ?? "";
        }
    }
}