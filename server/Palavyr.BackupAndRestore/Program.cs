using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
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
        
        static IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<BackupAndRestoreApp>();
            builder.RegisterType<Executor>().AsSelf();
            builder = Customizer.CustomizeContainer(builder);
            return builder.Build();
        }
    }
}