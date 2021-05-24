#nullable enable
using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.FixtureBase;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static class CreateAccountBuilder
    {
        public static DefaultAccountAndSessionBuilder CreateDefaultAccountAndSessionBuilder(this InMemoryIntegrationFixture test)
        {
            return new DefaultAccountAndSessionBuilder(test);
        }
    }

    public class DefaultAccountAndSessionBuilder
    {
        private readonly InMemoryIntegrationFixture test;
        private string? emailAddress;
        private string? password;
        private string? accountId;
        private AccountType? accountType;
        private string? apikey;

        public DefaultAccountAndSessionBuilder(InMemoryIntegrationFixture test)
        {
            this.test = test;
        }

        public DefaultAccountAndSessionBuilder WithDefaultAccountId()
        {
            this.accountId = IntegrationConstants.AccountId;
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

        public DefaultAccountAndSessionBuilder WithDefaultApiKey()
        {
            this.apikey = IntegrationConstants.ApiKey;
            return this;
        }

        public async Task<Account> Build()
        {
            var email = this.emailAddress ?? "test@gmail.com";
            var pass = this.password ?? "123456";
            var id = this.accountId ?? "account-123";
            var accType = this.accountType ?? AccountType.Default;
            var apiKey = this.apikey ?? "";

            var defaultAccount = Account.CreateAccount(email, pass, id, apiKey, accType);
            await test.AccountsContext.AddAsync(defaultAccount);
            test.AccountsContext.SeedSession(IntegrationConstants.AccountId, null);
            await test.AccountsContext.SaveChangesAsync();
            return defaultAccount;
        }
    }
}