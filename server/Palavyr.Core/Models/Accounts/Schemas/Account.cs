#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Palavyr.Core.Common.UIDUtils;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class Account
    {
        [Key] public int? Id { get; set; }
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
        public PaymentIntervalEnum PaymentInterval { get; set; }
        public bool HasUpgraded { get; set; }
        public string? StripeCustomerId { get; set; }
        public DateTime CurrentPeriodEnd { get; set; }

        [NotMapped] public readonly string DefaultLocale = "en-AU";


        public class PlanTypes
        {
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
            Premium = 1,
            Pro = 2
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
            AccountType accountType,
            PlanTypeEnum planType,
            PaymentIntervalEnum paymentInterval,
            bool hasUpgraded
        )
        {
            Password = password;
            EmailAddress = emailAddress;
            DefaultEmailIsVerified = false;
            AccountId = accountId;
            ApiKey = apiKey;
            CreationDate = DateTime.Now;
            CompanyName = companyName;
            PhoneNumber = phoneNumber;
            Active = active;
            Locale = locale;
            AccountType = accountType;
            PlanType = planType;
            PaymentInterval = paymentInterval;
            HasUpgraded = hasUpgraded;
            StripeCustomerId = null;
        }

        public static Account CreateGoogleAccount(
            string apikey,
            string emailAddress,
            string accountId,
            string locale
        )
        {
            return new Account
            {
                EmailAddress = emailAddress,
                Password = null,
                AccountId = accountId,
                ApiKey = apikey,
                CompanyName = null,
                PhoneNumber = null,
                Locale = locale,
                AccountType = AccountType.Google,
                PlanType = PlanTypeEnum.Free,
                PaymentInterval = PaymentIntervalEnum.Null,
                GeneralFallbackSubject = "",
                GeneralFallbackEmailTemplate = "",
            };
        }

        public static Account CreateAccount(
            string emailAddress,
            string password,
            string accountId,
            AccountType accountType
        )
        {
            return new Account(
                emailAddress, password, accountId, null, null, null, false,
                "en-AU",
                accountType, PlanTypeEnum.Free, PaymentIntervalEnum.Null, false);
        }

        public static Account CreateAccount(
            string emailAddress,
            string password,
            string accountId,
            string apiKey,
            AccountType accountType
        )
        {
            return new Account(
                emailAddress, password, accountId, apiKey, null, null, false,
                "en-AU",
                accountType, PlanTypeEnum.Free, PaymentIntervalEnum.Null, false);
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
            string locale,
            AccountType accountType
        )
        {
            return new Account(
                emailAddress, password, accountId, apiKey, companyName, phoneNumber,
                active, locale, accountType, PlanTypeEnum.Free, PaymentIntervalEnum.Null, false);
        }
    }
}