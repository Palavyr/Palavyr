using System;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Stores;
using Test.Common.Random;

namespace Test.Common.Builders.Accounts
{
    public class AccountObjectBuilder
    {
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

        public AccountObjectBuilder()
        {
        }

        public AccountObjectBuilder AsActive()
        {
            this.asActive = true;
            return this;
        }

        public AccountObjectBuilder WithAccountId(string accountId)
        {
            this.accountId = accountId;
            return this;
        }


        public AccountObjectBuilder WithEmailAddress(string email)
        {
            this.emailAddress = email;
            return this;
        }

        public AccountObjectBuilder WithDefaultPassword()
        {
            this.password = "Password01!";
            return this;
        }

        public AccountObjectBuilder WithDefaultAccountType()
        {
            this.accountType = AccountType.Default;
            return this;
        }

        public AccountObjectBuilder WithApiKey(string? apiKey)
        {
            this.apikey = apiKey;
            return this;
        }

        public AccountObjectBuilder WithProPlan()
        {
            this.planType = Account.PlanTypeEnum.Pro;
            this.currentPeriodEnd = Convert.ToDateTime(futureDate);
            this.paymentInterval = Account.PaymentIntervalEnum.Month;
            this.hasUpgrade = true;
            return this;
        }

        public AccountObjectBuilder WithPremiumPlan()
        {
            this.planType = Account.PlanTypeEnum.Premium;
            this.currentPeriodEnd = Convert.ToDateTime(futureDate);
            this.paymentInterval = Account.PaymentIntervalEnum.Month;
            this.hasUpgrade = true;

            return this;
        }

        public AccountObjectBuilder WithLytePlan()
        {
            this.planType = Account.PlanTypeEnum.Lyte;
            this.currentPeriodEnd = Convert.ToDateTime(futureDate);
            this.paymentInterval = Account.PaymentIntervalEnum.Month;
            this.hasUpgrade = true;

            return this;
        }

        public AccountObjectBuilder WithFreePlan()
        {
            this.planType = Account.PlanTypeEnum.Free;
            this.currentPeriodEnd = Convert.ToDateTime(pastDate);
            this.paymentInterval = Account.PaymentIntervalEnum.Null;
            this.hasUpgrade = false;

            return this;
        }

        public AccountObjectBuilder WithStripeCustomerId(string id)
        {
            this.customerId = id;
            return this;
        }

        public Account Build()
        {
            var email = this.emailAddress ?? A.RandomTestEmail();
            var pass = this.password ?? "123456";
            var id = this.accountId ?? A.RandomId();
            var accType = this.accountType ?? AccountType.Default;
            var active = this.asActive ?? false;
            var custId = this.customerId ?? A.RandomId();
            var payinterval = this.paymentInterval ?? Account.PaymentIntervalEnum.Null;
            var hasUpgraded = this.hasUpgrade ?? false;
            var planT = this.planType ?? Account.PlanTypeEnum.Free;
            var periodEnd = this.currentPeriodEnd ?? DateTime.UtcNow;
            var apiKey = this.apikey ?? A.RandomId();

            return new Account
            {
                EmailAddress = email,
                Password = pass,
                AccountId = id,
                ApiKey = apiKey,
                AccountType = accType,
                StripeCustomerId = custId,
                PhoneNumber = string.Empty,
                Locale = "en-AU",
                HasUpgraded = hasUpgraded,
                PaymentInterval = payinterval,
                PlanType = planT,
                Active = active,
                CurrentPeriodEnd = periodEnd,
                IntroIntentId = A.RandomId(),
                AccountLogoUri = string.Empty,
                CompanyName = string.Empty,
                GeneralFallbackSubject = string.Empty,
                GeneralFallbackEmailTemplate = string.Empty,
                CreationDate = DateTime.Now,
                ShowSeenEnquiries = false,
                DefaultEmailIsVerified = true
            };
        }

        public async Task<Account> BuildAndMake(IEntityStore<Account> accountStore)
        {
            var account = await accountStore.Create(Build());
            return account;
        }

        public async Task<Account> BuildAndMakeRaw(AppDataContexts rawDataContext, CancellationToken cancellationToken)
        {
            var account = Build();
            rawDataContext.Accounts.Add(account);
            await rawDataContext.SaveChangesAsync(cancellationToken);
            return account;
        }
    }
}