using System.ComponentModel.DataAnnotations;

namespace Palavyr.Core.Models.Accounts.Schemas
{
    public class Backup
    {
        [Key] public int Id { get; set; }
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
        public static Backup UpdateLatestDb(string latestDbBackup)
        {
            return new Backup()
            {
                LatestFullDbBackup = latestDbBackup,
            };
        }

        public static Backup UpdateLatestUserData(string latestUserDataBackup)
        {
            return new Backup()
            {
                LatestUserDataBackup = latestUserDataBackup
            };
        }
    }
}