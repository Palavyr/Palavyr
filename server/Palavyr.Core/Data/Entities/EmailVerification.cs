
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class EmailVerification : Entity, IHaveAccountId
    {
        public string AuthenticationToken { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string AccountId { get; set; } = null!;

        public EmailVerification()
        {
        }

        public static EmailVerification CreateNew(string authenticationToken, string emailAddress, string accountId)
        {
            return new EmailVerification()
            {
                AuthenticationToken = authenticationToken,
                EmailAddress = emailAddress,
                AccountId = accountId
            };
        }

    }
}