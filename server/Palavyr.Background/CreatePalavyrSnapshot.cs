using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.Background
{
    public class CreatePalavyrSnapshot : ICreatePalavyrSnapshot
    {
        private readonly IPostgresBackup postgresBackup;
        private readonly IUserDataBackup userDataBackup;
        private readonly IConfiguration configuration;

        private const string PostgresHost = "Postgres:host";
        private const string PostgresPort = "Postgres:port";
        private const string PostgresPassword = "Postgres:password";

        public CreatePalavyrSnapshot(IPostgresBackup postgresBackup, IUserDataBackup userDataBackup, IConfiguration configuration)
        {
            this.postgresBackup = postgresBackup;
            this.userDataBackup = userDataBackup;
            this.configuration = configuration;
        }

        public async Task CreateAndTransferCompleteBackup()
        {
            var snapshotTimeStamp = DateTime.Now.ToString(TimeUtils.DateTimeFormat);
            var host = configuration.GetSection(PostgresHost).Value;
            var port = configuration.GetSection(PostgresPort).Value;
            var pass = configuration.GetSection(PostgresPassword).Value;

            await postgresBackup.CreateFullDatabaseBackup(host, port, pass, snapshotTimeStamp);
            await userDataBackup.CreateFullUserDataBackup(snapshotTimeStamp);
        }
    }
}