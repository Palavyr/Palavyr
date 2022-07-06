#nullable disable

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class Account : Entity, IHaveAccountId
    {
        public string? Password { get; set; }
        public string EmailAddress { get; set; }
        public bool DefaultEmailIsVerified { get; set; }
        public string AccountId { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string? AccountLogoUri { get; set; }
        public string? GeneralFallbackSubject { get; set; }
        public string? GeneralFallbackEmailTemplate { get; set; }

        public string ApiKey { get; set; }
        public bool Active { get; set; }
        public string Locale { get; set; } = "en-AU";
        public AccountType AccountType { get; set; }
        public PlanTypeEnum PlanType { get; set; } = PlanTypeEnum.Free;
        public PaymentIntervalEnum? PaymentInterval { get; set; }
        public bool HasUpgraded { get; set; }
        public string? StripeCustomerId { get; set; }
        public DateTime CurrentPeriodEnd { get; set; }
        public string IntroductionId { get; set; }

        public bool ShowSeenEnquiries { get; set; }

        [NotMapped]
        public readonly string DefaultLocale = "en-AU";

        public class PlanTypes
        {
            public const string Lyte = "Lyte";
            public const string Premium = "Premium";
            public const string Pro = "Pro";
        }

        public class PaymentIntervals
        {
            public const string Month = "month";
            public const string Year = "year";
        }

        public enum PlanTypeEnum // ORDER IS CRITICAL. DO NOT CHANGE.
        {
            Free = 0,
            Lyte = 1,
            Premium = 2,
            Pro = 3
        }

        public enum PaymentIntervalEnum
        {
            Null = 0,
            Month = 1,
            Year = 2
        }

        public Account()
        {
        }

        private Account(
            string emailAddress,
            string password,
            string accountId,
            string? apiKey,
            string? companyName,
            string? phoneNumber,
            bool active,
            string locale,
            PlanTypeEnum planType,
            PaymentIntervalEnum paymentInterval,
            bool hasUpgraded
        )
        {
            Password = password;
            EmailAddress = emailAddress.ToLowerInvariant();
            DefaultEmailIsVerified = false;
            AccountId = accountId;
            ApiKey = apiKey;
            CreationDate = DateTime.Now;
            CompanyName = companyName;
            PhoneNumber = phoneNumber;
            Active = active;
            Locale = locale;
            AccountType = AccountType.Default;
            PlanType = planType;
            PaymentInterval = paymentInterval;
            HasUpgraded = hasUpgraded;
            StripeCustomerId = null;
            IntroductionId = new GuidUtils().CreateNewId();
        }

        public static Account CreateAccount(
            string emailAddress,
            string password,
            string accountId,
            string apiKey)
        {
            return CreateAccount(
                emailAddress,
                password,
                accountId,
                apiKey,
                null
            );
        }


        public static Account CreateAccount(
            string emailAddress,
            string password,
            string accountId,
            string apiKey,
            string? stripeCustomerId
        )
        {
            return new Account(
                emailAddress.ToLowerInvariant(), password, accountId, apiKey, null, null, false,
                "en-AU", PlanTypeEnum.Free, PaymentIntervalEnum.Null, false)
            {
                StripeCustomerId = stripeCustomerId
            };
        }

        public static Account CreateAccount(
            string userName,
            string emailAddress,
            string password,
            string accountId,
            string apiKey,
            string companyName,
            string phoneNumber,
            bool active,
            string locale
        )
        {
            return new Account(
                emailAddress.ToLowerInvariant(), password, accountId, apiKey, companyName, phoneNumber,
                active, "en-AU", PlanTypeEnum.Free, PaymentIntervalEnum.Null, false);
        }
    }
}