#nullable disable

using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class Subscription : Entity, IHaveAccountId
    {
        public string AccountId { get; set; }
        public string ApiKey { get; set; }
        public int NumIntents { get; set; }

        public Subscription()
        {
        }

        private Subscription(string accountId, string apiKey, int numIntents)
        {
            AccountId = accountId;
            ApiKey = apiKey;
            NumIntents = numIntents;
        }

        public static Subscription CreateNew(string accountId, string apiKey, int numIntents)
        {
            return new Subscription(accountId, apiKey, numIntents);
        }
    }
}