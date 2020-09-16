using System;
using System.Linq;
using DashboardServer.API.ReceiverTypes;
using DashboardServer.API.ResponseTypes;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.AccountDB;
using Microsoft.Extensions.Logging;


namespace DashboardServer.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class Authentication : BaseController
    {
        private static ILogger<Authentication> _logger;

        public Authentication(ILogger<Authentication> logger, AccountsContext accountContext, ConvoContext convoContext,
            DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
        }
        
        [HttpPost("login")]
        public Credentials PerformLogin([FromHeader] string action, LoginCredentials credentials)
        {
            if (action != MagicString.LoginAction) throw new Exception();
            
            // take credentials and check against the database
            var byUsername = AccountContext.Accounts.SingleOrDefault(row => row.UserName == credentials.Username);
            var byEmail = AccountContext.Accounts.SingleOrDefault(row => row.EmailAddress == credentials.EmailAddress);

            var userAccount = byUsername ?? byEmail;
            if (userAccount == null) return Credentials.CreateUnauthenticatedResponse("Could not find user.");
            
            if (!PasswordHashing.ComparePasswords(userAccount.Password, credentials.Password))
            {
                return Credentials.CreateUnauthenticatedResponse("Password does not match.");
            }

            var newSession = Session.CreateNew(userAccount.AccountId, userAccount.ApiKey);
            AccountContext.Sessions.Add(newSession);
            AccountContext.SaveChanges();
                
            return Credentials.CreateAuthenticatedResponse(newSession.SessionId, newSession.ApiKey);
        }

        [HttpPost("session/{sessionId}")]
        public bool CheckSession([FromHeader] string action, string sessionId)
        {
            if (action != MagicString.LoginAction) throw new Exception();

            var result = AccountContext.Sessions.SingleOrDefault(row => row.SessionId == sessionId);
            return result != null && result.Expiration < DateTime.Now;
        }
    }
}