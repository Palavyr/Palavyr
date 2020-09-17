using Palavyr.Common.uniqueIdentifiers;

namespace Palavyr.API.Controllers
{
    public class FakeDataHolder
    {
        public FakeDataHolder(string rawPassword, string accountId, string apiKey, string email, string userName,
            string companyName, string phoneNumber, bool active, string locale)
        {
            RawPassword = rawPassword;
            HashedPassword = PasswordHashing.CreateHashedPassword(rawPassword);
            AccountId = accountId;
            ApiKey = apiKey;
            Email = email;
            UserName = userName;
            CompanyName = companyName;
            PhoneNumber = phoneNumber;
            Active = active;
            Locale = locale;
        }

        public string Locale { get; }

        public bool Active { get; }

        public string PhoneNumber { get; }

        public string CompanyName { get; }

        public string UserName { get; }

        public string Email { get; }

        public string ApiKey { get; }

        public string AccountId { get; }

        public string HashedPassword { get; }

        private string RawPassword { get; }
    }
}