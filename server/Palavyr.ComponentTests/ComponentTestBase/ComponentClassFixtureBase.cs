using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.API;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Serilog;

namespace Palavyr.Component.ComponentTestBase
{
    public class ComponentClassFixture
    {
        public IContainer ConfigureClassFixture(Action<ContainerBuilder> additionalConfiguration)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", false);
            var config = configurationBuilder.Build();

            var services = new ServiceCollection();
            services.AddLogging(
                logger =>
                {
                    logger.AddSerilog();
                    logger.AddSeq();
                });

            WireUpDatabases(services);
            RegisterServices(services, config);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            RegisterContainerTypes(builder, config);
            builder.RegisterInstance(config).As<IConfiguration>();
            additionalConfiguration(builder);
            return builder.Build();
        }


        public void RegisterServices(IServiceCollection serviceCollection, IConfiguration config)
        {
            Startup.NonWebHostConfiguration(serviceCollection, config);
        }

        public void RegisterContainerTypes(ContainerBuilder builder, IConfiguration config)
        {
            Startup.ContainerSetup(builder, config);
        }

        private static void WireUpDatabases(IServiceCollection services)
        {
            ClearDescriptors(services);
            ConfigureInMemoryDatabases(services);
            CreateDatabases(services);
        }


        private static void CreateDatabases(IServiceCollection services)
        {
            var sp = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var dataContexts = scopedServices.GetService<AppDataContexts>();
            dataContexts.Database.EnsureDeleted();
            dataContexts.Database.EnsureCreated();
        }

        private static void ClearDescriptors(IServiceCollection services)
        {
            var descriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDataContexts>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

        private static void ConfigureInMemoryDatabases(IServiceCollection services)
        {
            var dbRoot = new InMemoryDatabaseRoot();
            var dbName = "TestDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);

            services.AddDbContext<AppDataContexts>(
                opt =>
                {
                    opt.UseInMemoryDatabase(dbName, dbRoot);
                    SuppressWarnings(opt);
                });
        }

        private static void SuppressWarnings(DbContextOptionsBuilder builder)
        {
            builder.ConfigureWarnings(x => { x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning); });
            builder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }
    }
}