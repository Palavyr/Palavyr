using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Configuration.Schemas
{
    public class FileNameMap
    {
        [Key]
        public int? Id { get; set; }
        public string SafeName { get; set; }
        public string RiskyName { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }

        private FileNameMap(string safeName, string riskyName, string accountId, string areaIdentifier)
        {
            SafeName = safeName;
            RiskyName = riskyName;
            AccountId = accountId;
            AreaIdentifier = areaIdentifier;
        }

        public static FileNameMap CreateFileMap(string safeName, string riskyName, string accountId, string areaIdentifier)
        {
            return new FileNameMap(safeName, riskyName, accountId, areaIdentifier);
        }
    }
}