using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Palavyr.Common.uniqueIdentifiers;

namespace Server.Domain.Accounts
{
    public class UserAccount
    {
        [Key] 
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public bool DefaultEmailIsVerified { get; set; }
        public string AccountId { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string AccountLogoUri { get; set; }

        public string ApiKey { get; set; }
        public bool Active { get; set; }
        public string Locale { get; set; } = "en-AU";
        public AccountType AccountType { get; set; }
        
        [NotMapped] public readonly string DefaultLocale = "en-AU";

        public UserAccount()
        {
            
        }
        private UserAccount(
            string userName, 
            string emailAddress, 
            string password, 
            string accountId, 
            string apiKey,
            string companyName, 
            string phoneNumber, 
            bool active, 
            string locale,
            AccountType accountType)
        {
            UserName = userName;
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
        }


        public static UserAccount CreateGoogleAccount(string userName, string emailAddress, string accountId, string locale)
        {
            return new UserAccount(userName, emailAddress, null, accountId, null, null, null, false, locale, AccountType.Google);
        }
        
        public static UserAccount CreateAccount(string userName, string emailAddress, string password, string accountId, AccountType accountType)
        {
            return new UserAccount(userName, emailAddress, password, accountId, null, null, null, false, "en-AU", accountType);
        }

        public static UserAccount CreateAccount(string userName, string emailAddress, string password, string accountId, string apiKey, AccountType accountType)
        {
            return new UserAccount(userName, emailAddress, password, accountId, apiKey, null, null, false, "en-AU", accountType);
        }

        public static UserAccount CreateAccount(string userName, string emailAddress, string password, string accountId,
            string apiKey, string companyName, string phoneNumber, bool active, string locale, AccountType accountType)
        {
            return new UserAccount(userName, emailAddress, password, accountId, apiKey, companyName, phoneNumber,
                active, locale, accountType);
        }
    }
}