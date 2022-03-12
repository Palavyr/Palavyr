#nullable enable
using System;
using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Sessions;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static class CreateAccountBuilder
    {
        public static DefaultAccountAndSessionBuilder CreateDefaultAccountAndSessionBuilder(this BaseIntegrationFixture test)
        {
            return new DefaultAccountAndSessionBuilder(test);
        }
    }

    public class DefaultAccountAndSessionBuilder
    {
        private readonly BaseIntegrationFixture test;
        private string? emailAddress;
        private string? password;
        private string? accountId;
        private AccountType? accountType;
        private string? apikey;
        private Account.PlanTypeEnum? planType;
        private bool? asActive;
        private string? customerId;
        private DateTime? currentPeriodEnd;
        private Account.PaymentIntervalEnum? paymentInterval;
        private bool? hasUpgrade;

        private readonly DateTime futureDate = DateTime.Parse("01/01/2200");
        private readonly DateTime pastDate = DateTime.Parse("01/01/2200");

        public DefaultAccountAndSessionBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public DefaultAccountAndSessionBuilder AsActive()
        {
            this.asActive = true;
            return this;
        }

        public DefaultAccountAndSessionBuilder WithAccountId(string accountId)
        {
            this.accountId = accountId;
            return this;
        }


        public DefaultAccountAndSessionBuilder WithDefaultEmailAddress()
        {
            this.emailAddress = this.test.EmailAddress;
            return this;
        }

        public DefaultAccountAndSessionBuilder WithDefaultPassword()
        {
            this.password = IntegrationConstants.Password;
            return this;
        }

        public DefaultAccountAndSessionBuilder WithDefaultAccountType()
        {
            this.accountType = AccountType.Default;
            return this;
        }

        public DefaultAccountAndSessionBuilder WithApiKey(string apiKey)
        {
            this.apikey = apiKey;
            return this;
        }

        public DefaultAccountAndSessionBuilder WithProPlan()
        {
            this.planType = Account.PlanTypeEnum.Pro;
            this.currentPeriodEnd = Convert.ToDateTime(futureDate);
            this.paymentInterval = Account.PaymentIntervalEnum.Month;
            this.hasUpgrade = true;
            return this;
        }

        public DefaultAccountAndSessionBuilder WithPremiumPlan()
        {
            this.planType = Account.PlanTypeEnum.Premium;
            this.currentPeriodEnd = Convert.ToDateTime(futureDate);
            this.paymentInterval = Account.PaymentIntervalEnum.Month;
            this.hasUpgrade = true;

            return this;
        }

        public DefaultAccountAndSessionBuilder WithLytePlan()
        {
            this.planType = Account.PlanTypeEnum.Lyte;
            this.currentPeriodEnd = Convert.ToDateTime(futureDate);
            this.paymentInterval = Account.PaymentIntervalEnum.Month;
            this.hasUpgrade = true;

            return this;
        }

        public DefaultAccountAndSessionBuilder WithFreePlan()
        {
            this.planType = Account.PlanTypeEnum.Free;
            this.currentPeriodEnd = Convert.ToDateTime(pastDate);
            this.paymentInterval = Account.PaymentIntervalEnum.Null;
            this.hasUpgrade = false;

            return this;
        }

        public DefaultAccountAndSessionBuilder WithStripeCustomerId(string id)
        {
            this.customerId = id;
            return this;
        }


        public async Task<Account> Build()
        {
            var email = this.emailAddress ?? "test@gmail.com";
            var pass = this.password ?? "123456";
            var id = this.accountId ?? test.AccountId;
            var accType = this.accountType ?? AccountType.Default;
            var active = this.asActive ?? false;
            var custId = this.customerId ?? test.StripeCustomerId;
            var payinterval = this.paymentInterval ?? Account.PaymentIntervalEnum.Null;
            var hasUpgraded = this.hasUpgrade ?? false;
            var planT = this.planType ?? Account.PlanTypeEnum.Free;
            var periodEnd = this.currentPeriodEnd ?? DateTime.UtcNow;

            var defaultAccount = new Account
            {
                EmailAddress = email,
                Password = pass,
                AccountId = id,
                ApiKey = test.ApiKey,
                AccountType = accType,
                StripeCustomerId = custId,
                PhoneNumber = null,
                Locale = "en-AU",
                HasUpgraded = hasUpgraded,
                PaymentInterval = payinterval,
                PlanType = planT,
                Active = active,
                CurrentPeriodEnd = periodEnd,
                IntroductionId = A.RandomId()
            };

            SetAccountIdTransportIfNotSet(); 
            await test.CreateAndSave(defaultAccount);
            await test.CreateAndSave(Session.CreateNew(test.SessionId, test.AccountId, test.ApiKey));
            

            return defaultAccount;
        }

        private void SetAccountIdTransportIfNotSet()
        {
            var accToken = test.ResolveType<IAccountIdTransport>();
            if (!accToken.IsSet())
            {
                test.SetAccountIdTransport();
            }
        }
    }
}