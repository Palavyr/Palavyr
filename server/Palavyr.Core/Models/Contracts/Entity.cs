using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Models.Contracts
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}