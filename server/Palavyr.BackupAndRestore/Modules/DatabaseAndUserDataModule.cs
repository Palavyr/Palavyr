using Autofac;
using EmailService.Verification;
using Palavyr.Amazon.S3Services;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;

namespace Palavyr.BackupAndRestore.Modules
{
    public class DatabaseAndUserDataModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<S3Retriever>().As<IS3Retriever>();
            builder.RegisterType<PostgresRestorer>().AsSelf();
            builder.RegisterType<UpdateDatabaseLatest>().AsSelf();
            builder.RegisterType<UserDataBackup>().As<IUserDataBackup>();
            builder.RegisterType<PostgresBackup>().As<IPostgresBackup>();
            builder.RegisterType<EmailVerificationStatus>().AsSelf();

        }    

    }
}