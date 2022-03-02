using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class FileAsset
    {
        [Key]
        public int? Id { get; set; }                // ID for the DB
        public string SafeNameStem { get; set; }    // A GUID assigned to this file
        public string RiskyNameStem { get; set; }   // The original name (no extension) of the uploaded file
        public string AccountId { get; set; }       // the accounts Id 
        public string Extension { get; set; }       // one of pdf, txt, png, svg, jpg, etc
        public string FileId { get; set; }          // this is what is referenced by other tables
        public string LocationKey { get; set; }     // this is path to a location in the storage provide
    }
}