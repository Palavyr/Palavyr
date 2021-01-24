using System.Threading.Tasks;
using Autofac;
using Palavyr.BackupAndRestore.Modules;

namespace Palavyr.BackupAndRestore
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CompositionRoot();
            var program = builder.Resolve<BackupAndRestoreApp>();
            await program.Execute(args);
        }

        private static IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<BackupAndRestoreApp>();
            builder.RegisterType<Operations>().AsSelf();
            builder = Customizer.CustomizeContainer(builder);
            return builder.Build();
        }
    }
}