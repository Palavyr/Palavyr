using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class StripeWebhookReveivedRecord : IEntity
    {
        public string RecordId { get; set; }
        public string PayloadSignature { get; set; }

        public StripeWebhookReveivedRecord()
        {
        }

        public static StripeWebhookReveivedRecord CreateNewRecord(string id, string payloadSignature)
        {
            return new StripeWebhookReveivedRecord
            {
                RecordId = id,
                PayloadSignature = payloadSignature
            };
        }

        int? IId.Id
        {
            get => id;
            set => id = value;
        }
    }
}