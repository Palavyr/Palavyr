namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class StripeWebhookRecord : Entity
    {
        public string RecordId { get; set; }
        public string PayloadSignature { get; set; }

        public StripeWebhookRecord()
        {
        }

        public static StripeWebhookRecord CreateNewRecord(string id, string payloadSignature)
        {
            return new StripeWebhookRecord
            {
                RecordId = id,
                PayloadSignature = payloadSignature
            };
        }
    }
}