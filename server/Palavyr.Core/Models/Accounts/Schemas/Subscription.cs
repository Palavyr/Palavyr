using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class Subscription : Entity, IHaveAccountId
    {
        public string AccountId { get; set; }
        public string ApiKey { get; set; }
        public int NumAreas { get; set; }

        public Subscription()
        {
        }

        private Subscription(string accountId, string apiKey, int numAreas)
        {
            AccountId = accountId;
            ApiKey = apiKey;
            NumAreas = numAreas;
        }

        public static Subscription CreateNew(string accountId, string apiKey, int numAreas)
        {
            return new Subscription(accountId, apiKey, numAreas);
        }
    }
}