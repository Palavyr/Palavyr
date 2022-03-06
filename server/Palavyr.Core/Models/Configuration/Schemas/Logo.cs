using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class Logo : IHaveAccountId, ISingleRowEntity
    {
        [Key]
        public int? Id { get; set; }
        public string AccountId { get; set; }
        public string AccountLogoFileId { get; set; }
    }
}