
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class UserSession : Entity, IHaveAccountId
    {
        public string SessionId { get; set; }
        public string AccountId { get; set; }
        public string ApiKey { get; set; }
        public DateTime Expiration { get; set; }

        [NotMapped] private readonly int ExpirationPeriod = 24;

        public UserSession()
        {
        }

        private UserSession(string sessionId, string accountId, string apiKey)
        {
            SessionId = sessionId;
            AccountId = accountId;
            ApiKey = apiKey;
            Expiration = DateTime.UtcNow.Add(TimeSpan.FromHours(ExpirationPeriod));
        }
        

        public static UserSession CreateNew(string token, string accountId, string apiKey)
        {
            return new UserSession(token, accountId, apiKey);
        }
        
        public static UserSession CreateNewValidationSession(string accountId, string confirmationToken)
        {
            return new UserSession(confirmationToken, accountId, "NON-KEY");
        }
    }
}