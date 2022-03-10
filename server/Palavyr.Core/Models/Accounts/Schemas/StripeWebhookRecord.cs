using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class StripeWebhookRecord : IEntity
    {
        [Key]
        public int? Id { get; set; }
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