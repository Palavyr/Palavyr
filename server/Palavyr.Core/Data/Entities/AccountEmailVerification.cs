
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class AccountEmailVerification : Entity, IHaveAccountId
    {
        public string AuthenticationToken { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string AccountId { get; set; } = null!;

        public AccountEmailVerification()
        {
        }

        public static AccountEmailVerification CreateNew(string authenticationToken, string emailAddress, string accountId)
        {
            return new AccountEmailVerification
            {
                AuthenticationToken = authenticationToken,
                EmailAddress = emailAddress,
                AccountId = accountId
            };
        }

    }
}