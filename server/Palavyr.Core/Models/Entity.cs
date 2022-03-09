using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class Entity : IEntity
    {
        [Key]
        public int? Id { get; set; }
    }
}