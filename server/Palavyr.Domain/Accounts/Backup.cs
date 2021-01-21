namespace DashboardServer.Data
{
    public class Backup
    {
        public string LatestDbBackup { get; set; }
        public string LatestUserDataBackup { get; set; }
        public Backup()
        {
        }

        public static Backup Create(string latestDbBackup, string latestUserDataBackup)
        {
            return new Backup()
            {
                LatestDbBackup = latestDbBackup,
                LatestUserDataBackup = latestUserDataBackup
            };
        }
    }
}