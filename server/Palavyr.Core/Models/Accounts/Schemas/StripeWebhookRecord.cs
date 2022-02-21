using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class StripeWebhookRecord
    {
        [Key]
        public string Id { get; set; }
        public string PayloadSignature { get; set; }

        public StripeWebhookRecord()
        {
        }

        public static StripeWebhookRecord CreateNewRecord(string id, string payloadSignature)
        {
            return new StripeWebhookRecord
            {
                Id = id,
                PayloadSignature = payloadSignature
            };
        }
    }
}