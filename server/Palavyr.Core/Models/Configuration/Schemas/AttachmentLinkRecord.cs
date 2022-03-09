using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class AttachmentLinkRecord : Entity
    {
        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public string FileId { get; set; }
    }
}