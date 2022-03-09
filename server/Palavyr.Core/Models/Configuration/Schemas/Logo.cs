using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class Logo : Entity, IHaveAccountId, ISingleRowEntity
    {
        public string AccountId { get; set; }
        public string AccountLogoFileId { get; set; }
    }
}