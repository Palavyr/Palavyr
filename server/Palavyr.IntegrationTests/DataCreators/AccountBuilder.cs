#nullable enable
using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;

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

        public DefaultAccountAndSessionBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public DefaultAccountAndSessionBuilder WithAccountId(string accountId)
        {
            this.accountId = accountId;
            return this;
        }

        public DefaultAccountAndSessionBuilder WithDefaultEmailAddress()
        {
            this.emailAddress = IntegrationConstants.EmailAddress;
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
            return this;
        }

        public DefaultAccountAndSessionBuilder WithPremiumPlan()
        {
            this.planType = Account.PlanTypeEnum.Premium;
            return this;
        }

        public DefaultAccountAndSessionBuilder WithLytePlan()
        {
            this.planType = Account.PlanTypeEnum.Lyte;
            return this;
        }

        public DefaultAccountAndSessionBuilder WithFreePlan()
        {
            this.planType = Account.PlanTypeEnum.Free;
            return this;
        }


        public async Task<Account> Build()
        {
            var email = this.emailAddress ?? "test@gmail.com";
            var pass = this.password ?? "123456";
            var id = this.accountId ?? "account-123";
            var accType = this.accountType ?? AccountType.Default;

            var defaultAccount = Account.CreateAccount(email, pass, id, test.ApiKey, accType);

            defaultAccount.PlanType = this.planType ?? Account.PlanTypeEnum.Free;

            await test.AccountsContext.Accounts.AddAsync(defaultAccount);
            var session = Session.CreateNew(IntegrationConstants.SessionId, test.AccountId, test.ApiKey);
            await test.AccountsContext.Sessions.AddAsync(session);
            await test.AccountsContext.SaveChangesAsync();
            return defaultAccount;
        }
    }
}