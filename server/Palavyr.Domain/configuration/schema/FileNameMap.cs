using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Configuration.schema
{
    public class FileNameMap
    {
        [Key]
        public int Id { get; set; }
        public string SafeName { get; set; }
        public string RiskyName { get; set; }
        public string AccountId { get; set; }
        public string AreaId { get; set; }

        private FileNameMap(string safeName, string riskyName, string accountId, string areaId)
        {
            SafeName = safeName;
            RiskyName = riskyName;
            AccountId = accountId;
            AreaId = areaId;
        }

        public static FileNameMap CreateFileMap(string safeName, string riskyName, string accountId, string areaId)
        {
            return new FileNameMap(safeName, riskyName, accountId, areaId);
        }
    }
}