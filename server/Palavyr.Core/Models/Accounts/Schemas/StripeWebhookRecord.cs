namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class StripeWebhookRecord
    {
        public string Id { get; set; }
        public string PayloadSignature { get; set; }
        
        public StripeWebhookRecord()
        {
        }

        public static StripeWebhookRecord CreateNewRecord(string eventId)
        {
            return new StripeWebhookRecord()
            {
                PayloadSignature = eventId
            };
        }
    }
}