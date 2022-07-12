using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static class CreateDefaultTestAccountBuilderExtensionMethods
    {
        public static DefaultTestAccountBuilder CreateDefaultTestAccountBuilder(this IntegrationTest testBase)
        {
            return new DefaultTestAccountBuilder(testBase);
        }
    }

    public class DefaultTestAccountBuilder
    {
        private readonly IntegrationTest testBase;


        public DefaultTestAccountBuilder(IntegrationTest testBase)
        {
            this.testBase = testBase;
        }

        public async Task<Credentials> Build(string email, string password)
        {
            var credentials = await testBase.Client.Post<CreateNewAccountRequest, Credentials>(
                new CreateNewAccountRequest
                {
                    EmailAddress = email,
                    Password = password
                }, testBase.CancellationToken);

            testBase.SessionId = credentials.SessionId;
            testBase.ApiKey = credentials.ApiKey;

            // TODO: If we make the client lazy again, does it break everything? bc if not, then we can just do this
            // testBase.Client.AddHeader(ApplicationConstants.MagicUrlStrings.SessionId, credentials.SessionId);

            // activate the account
            await testBase.Client.Post<ConfirmEmailAddressRequest, bool>(
                testBase.CancellationToken,
                r => ConfirmEmailAddressRequest.FormatRoute(IntegrationTest.ConfirmationToken));
            
            // with the mocks registered, we update the account to Pro
            // the upgrade path is deliberately hard - it only happens via the stripe webhooks
            await testBase.Client.Post<ProcessStripeNotificationWebhookRequest, Unit>(testBase.CancellationToken);


            return credentials;
        }
    }


    public static class CreateAccountBuilder
    {
        public static DefaultAccountAndSessionBuilder CreateDefaultAccountAndSessionBuilder(this IntegrationTest testBase)
        {
            return new DefaultAccountAndSessionBuilder(testBase);
        }
    }

    public class DefaultAccountAndSessionBuilder
    {
        private readonly IntegrationTest testBase;
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

        public DefaultAccountAndSessionBuilder(IntegrationTest testBase)
        {
            this.testBase = testBase;
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
            this.emailAddress = this.testBase.EmailAddress;
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

        public DefaultAccountAndSessionBuilder WithApiKey(string? apiKey)
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

        // public DefaultAccountAndSessionBuilder WithFreePlan()
        // {
        //     this.planType = Account.PlanTypeEnum.Free;
        //     this.currentPeriodEnd = Convert.ToDateTime(pastDate);
        //     this.paymentInterval = Account.PaymentIntervalEnum.Null;
        //     this.hasUpgrade = false;
        //
        //     return this;
        // }

        public DefaultAccountAndSessionBuilder WithStripeCustomerId(string id)
        {
            this.customerId = id;
            return this;
        }

        public async Task<Credentials> BuildAndMake()
        {
            var email = this.emailAddress ?? "test@gmail.com";
            var pass = this.password ?? "123456";

            var newAccount = await testBase.Client.Post<CreateNewAccountRequest, CreateNewAccountResponse>(
                new CreateNewAccountRequest
                {
                    EmailAddress = email,
                    Password = pass
                }, testBase.CancellationToken);

            if (planType != null)
            {
                var accountStore = testBase.ResolveStore<Account>();
                var account = await accountStore
                    .DangerousRawQuery()
                    .SingleOrDefaultAsync(x => x.AccountId == testBase.AccountId);
            }


            return newAccount.Response;
        }
    }
}