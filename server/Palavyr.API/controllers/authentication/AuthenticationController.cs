using System;
using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.ResponseTypes;
using Palavyr.Common.requests;
using Palavyr.Common.uniqueIdentifiers;
using Server.Domain.Accounts;


namespace Palavyr.API.Controllers
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
            if (action != MagicUrlStrings.LoginAction) throw new Exception();

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
            if (action != MagicUrlStrings.LoginAction) throw new Exception();

            var result = AccountContext.Sessions.SingleOrDefault(row => row.SessionId == sessionId);
            return result != null && result.Expiration < DateTime.Now;
        }

        [HttpPost("sessionlogin")]
        public Credentials PerformSessionLogin([FromHeader] string action, LoginCredentials credentials)
        {
            // lookup sessionID - is it valid? retrieve accountID
            var sessionToken = credentials.sessionToken;
            if (sessionToken == null) 
                return Credentials.CreateUnauthenticatedResponse("No active Session.");
            var session = AccountContext.Sessions.SingleOrDefault(row => row.SessionId == sessionToken);
            
            if (session == null)
            {
                return Credentials.CreateUnauthenticatedResponse("Session not found.");
            }
            // if expired or non existent
            if (session.Expiration < DateTime.Now | session == null)
            {
                // TODO: DOES THIS WORK RIGHT? {
                if (session != null)
                {
                    AccountContext.Sessions.Remove(session);
                    AccountContext.SaveChanges();
                }

                return Credentials.CreateUnauthenticatedResponse("Session Id Expired.");
            }

            var account = AccountContext.Accounts.SingleOrDefault(row => row.EmailAddress == credentials.EmailAddress);
            if (credentials.EmailAddress != account.EmailAddress)
                return Credentials.CreateUnauthenticatedResponse("Current email address does not match the session Id");

            // // remove old session and create a new session
            // AccountContext.Sessions.Remove(session);
            // var newSession = Session.CreateNew(session.AccountId, session.ApiKey);
            // AccountContext.Sessions.Add(newSession);

            // save changes
            AccountContext.SaveChanges();
            return Credentials.CreateAuthenticatedResponse(session.SessionId, session.ApiKey);
        }
    }
}