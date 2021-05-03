using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class Backup
    {
        [Key] public int Id { get; set; }
        public string LatestFullDbBackup { get; set; }

        public Backup()
        {
        }

        public static Backup Create(string latestDbBackup)
        {
            return new Backup()
            {
                LatestFullDbBackup = latestDbBackup,
            };
        }
    }
}