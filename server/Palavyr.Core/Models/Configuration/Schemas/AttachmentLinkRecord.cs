using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class AttachmentLinkRecord
    {
        [Key]
        public int? Id { get; set; }

        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public string FileId { get; set; }
    }
}