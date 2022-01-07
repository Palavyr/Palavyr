using System;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class CreateDefaultAccountExtensionMethods
    {
        public static void SeedSession(this AccountsContext accountsContext, string accountId, string apiKey)
        {
            var session = Session.CreateNew(IntegrationConstants.SessionId, accountId, apiKey);
            accountsContext.Sessions.Add(session);
            accountsContext.SaveChanges();
        }

    }
}