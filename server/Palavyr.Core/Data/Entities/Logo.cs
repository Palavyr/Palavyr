

using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class Logo : Entity, IHaveAccountId, ISingleRowEntity
    {
        public string AccountId { get; set; }
        public string AccountLogoFileId { get; set; }
    }
}