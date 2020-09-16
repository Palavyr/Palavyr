using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.AccountDB
{
    public class Session
    {
        public string SessionId { get; set; }
        public string AccountId { get; set; }
        public string ApiKey { get; set; }
        public DateTime Expiration { get; set; }

        [NotMapped] private readonly int ExpirationPeriod = 24;

        Session(string sessionId, string accountId, string apiKey)
        {
            SessionId = sessionId;
            AccountId = accountId;
            ApiKey = apiKey;
            Expiration = DateTime.Now.Add(TimeSpan.FromHours(ExpirationPeriod));
        }

        public static Session CreateNew(string accountId, string apiKey)
        {
            var newSessionId = Guid.NewGuid().ToString();
            return new Session(newSessionId, accountId, apiKey);
        }

        public static Session CreateNewValidationSession(string accountId, string confirmationToken)
        {
            return new Session(confirmationToken, accountId, "NON-KEY");
        }
    }
}