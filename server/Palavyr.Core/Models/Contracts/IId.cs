using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Models.Contracts
{
    public interface IId
    {
        [Key]
        public int Id { get; set; }
    }
}