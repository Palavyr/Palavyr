using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class EmailVerification : IEntity
    {
        [Key]
        public int? Id { get; set; }
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