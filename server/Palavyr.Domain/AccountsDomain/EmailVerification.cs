using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Accounts
{
    public class EmailVerification
    {
        
        [Key]
        public int? Id { get; set; }
        public string AuthenticationToken { get; set; }
        public string EmailAddress { get; set; }
        public string AccountId { get; set; }

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