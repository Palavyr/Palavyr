#nullable disable

using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class AttachmentLinkRecord : Entity, IHaveAccountId
    {
        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public string FileId { get; set; }
    }
}