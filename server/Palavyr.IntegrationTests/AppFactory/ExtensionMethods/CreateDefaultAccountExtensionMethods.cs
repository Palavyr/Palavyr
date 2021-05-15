using System;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class CreateDefaultAccountExtensionMethods
    {
        public static void SetupFullAccount(this AccountsContext accountsContext)
        {
            accountsContext.CreateCompleteDefaultAccount();
            accountsContext.SeedSession(IntegrationConstants.AccountId, IntegrationConstants.ApiKey);
        }

        public static void SeedSession(this AccountsContext accountsContext, string accountId, string apiKey)
        {
            var session = Session.CreateNew(IntegrationConstants.SessionId, accountId, apiKey);
            accountsContext.Sessions.Add(session);
            accountsContext.SaveChanges();
        }

        public static void CreateDefaultMinimalAccount(this AccountsContext accountContext)
        {
            var account = Account.CreateAccount(
                IntegrationConstants.EmailAddress,
                IntegrationConstants.Password,
                IntegrationConstants.AccountId,
                IntegrationConstants.ApiKey,
                AccountType.Default
            );

            accountContext.Accounts.Add(account);
            accountContext.SaveChanges();
        }

        public static void CreateCompleteDefaultAccount(this AccountsContext accountsContext)
        {
            accountsContext.Accounts.Add(
                new Account
                {
                    AccountId = IntegrationConstants.AccountId,
                    StripeCustomerId = "test_StripeID_23423424",
                    AccountLogoUri = "",
                    AccountType = AccountType.Default,
                    Active = true,
                    ApiKey = IntegrationConstants.ApiKey,
                    CompanyName = "TestCompany",
                    CreationDate = DateTime.Now,
                    CurrentPeriodEnd = DateTime.Now.AddMonths(1),
                    DefaultEmailIsVerified = true,
                    EmailAddress = "test@gmail.com",
                    PlanType = Account.PlanTypeEnum.Free,
                    GeneralFallbackSubject = "TestSubject",
                    GeneralFallbackEmailTemplate = "TestTemplate",
                    HasUpgraded = false,
                    PaymentInterval = Account.PaymentIntervalEnum.Month,
                    PhoneNumber = "",
                    Locale = "en-AU"
                });

            accountsContext.Sessions.Add(
                new Session
                {
                    AccountId = IntegrationConstants.AccountId,
                    ApiKey = IntegrationConstants.ApiKey,
                    Expiration = DateTime.Now.AddDays(10),
                    SessionId = IntegrationConstants.SessionId
                });
        }
    }
}