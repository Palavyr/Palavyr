#nullable disable

using System.ComponentModel.DataAnnotations.Schema;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class FileAsset : Entity, IHaveAccountId
    {
        public string AccountId { get; set; } // the accounts Id 
        public string FileId { get; set; } // A Guid referenced by other tables
        public string RiskyNameStem { get; set; } // The original name (no extension) of the uploaded file
        public string Extension { get; set; } // one of pdf, txt, png, svg, jpg, etc
        public string LocationKey { get; set; } // this is path to a location in the storage provide

        [NotMapped]
        public string RiskyNameWithExtension => string.Join("", RiskyNameStem, Extension);
    }
}