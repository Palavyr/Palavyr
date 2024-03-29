
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class StripeWebhookReceivedRecord : Entity
    {
        public string RecordId { get; set; }
        public string PayloadSignature { get; set; }

        public StripeWebhookReceivedRecord()
        {
        }

        public static StripeWebhookReceivedRecord CreateNewRecord(string id, string payloadSignature)
        {
            return new StripeWebhookReceivedRecord
            {
                RecordId = id,
                PayloadSignature = payloadSignature
            };
        }
    }
}