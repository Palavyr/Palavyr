using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Accounts
{
    public class UserAccount
    {
        [Key] 
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string AccountId { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string AccountLogoUri { get; set; }

        public string ApiKey { get; set; }
        public bool Active { get; set; }
        public string Locale { get; set; } = "en-AU";

        [NotMapped] public readonly string DefaultLocale = "en-AU";

        private UserAccount(
            string userName, 
            string emailAddress, 
            string password, 
            string accountId, 
            string apiKey,
            string companyName, 
            string phoneNumber, 
            bool active, 
            string locale)
        {
            UserName = userName;
            Password = password;
            EmailAddress = emailAddress;
            AccountId = accountId;
            ApiKey = apiKey;
            CreationDate = DateTime.Now;
            CompanyName = companyName;
            PhoneNumber = phoneNumber;
            Active = active;
            Locale = locale;
        }

        public static UserAccount CreateAccount(string userName, string emailAddress, string password, string accountId)
        {
            return new UserAccount(userName, emailAddress, password, accountId, null, null, null, false, "en-AU");
        }

        public static UserAccount CreateAccount(string userName, string emailAddress, string password, string accountId, string apiKey)
        {
            return new UserAccount(userName, emailAddress, password, accountId, apiKey, null, null, false, "en-AU");
        }

        public static UserAccount CreateAccount(string userName, string emailAddress, string password, string accountId,
            string apiKey, string companyName, string phoneNumber, bool active, string locale)
        {
            return new UserAccount(userName, emailAddress, password, accountId, apiKey, companyName, phoneNumber,
                active, locale);
        }
    }
}