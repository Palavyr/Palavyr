using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Accounts
{
    public class Backup
    {
        [Key] public int? Id { get; set; }
        public string LatestFullDbBackup { get; set; }
        public string LatestUserDataBackup { get; set; }
        public Backup()
        {
        }

        public static Backup Create(string latestDbBackup, string latestUserDataBackup)
        {
            return new Backup()
            {
                LatestFullDbBackup = latestDbBackup,
                LatestUserDataBackup = latestUserDataBackup
            };
        }
    }
}