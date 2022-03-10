using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class AttachmentLinkRecord : IEntity
    {
        [Key]
        public int? Id { get; set; }
        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public string FileId { get; set; }
    }
}