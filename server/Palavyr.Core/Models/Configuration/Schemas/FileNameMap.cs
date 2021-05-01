using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class FileNameMap : IRecord
    {
        [Key]
        public int? Id { get; set; }
        public string SafeName { get; set; }
        public string S3Key { get; set; }
        public string RiskyName { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }

        public FileNameMap()
        {
        }
        
        private FileNameMap(string safeName, string riskyName, string s3Key, string accountId, string areaIdentifier)
        {
            SafeName = safeName;
            S3Key = s3Key;
            RiskyName = riskyName;
            AccountId = accountId;
            AreaIdentifier = areaIdentifier;
        }

        public static FileNameMap CreateFileMap(string safeName, string riskyName, string s3Key, string accountId, string areaIdentifier)
        {
            return new FileNameMap(safeName, riskyName, s3Key, accountId, areaIdentifier);
        }
    }
}