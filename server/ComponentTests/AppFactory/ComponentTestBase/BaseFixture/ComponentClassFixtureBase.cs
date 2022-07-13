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

namespace Component.AppFactory.ComponentTestBase.BaseFixture
{
    public class ComponentClassFixture
    {
        public ComponentClassFixture()
        {
        }

        public IContainer ConfigureClassFixture(Action<ContainerBuilder> additionalConfiguration)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", true);
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

            var dashContext = scopedServices.GetService<DashContext>();
            var accountContext = scopedServices.GetService<AccountsContext>();
            var convoContext = scopedServices.GetService<ConvoContext>();

            accountContext.Database.EnsureDeleted();
            dashContext.Database.EnsureDeleted();
            convoContext.Database.EnsureDeleted();

            accountContext.Database.EnsureCreated();
            dashContext.Database.EnsureCreated();
            convoContext.Database.EnsureCreated();
        }

        private static void ClearDescriptors(IServiceCollection services)
        {
            var accountsContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AccountsContext>));

            if (accountsContextDescriptor != null)
            {
                services.Remove(accountsContextDescriptor);
            }

            var dashContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DashContext>));
            if (dashContextDescriptor != null)
            {
                services.Remove(dashContextDescriptor);
            }

            var convoContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ConvoContext>));
            if (convoContextDescriptor != null)
            {
                services.Remove(convoContextDescriptor);
            }
        }

        private static void ConfigureInMemoryDatabases(IServiceCollection services)
        {
            var dbRoot = new InMemoryDatabaseRoot();
            var accountDbName = "TestAccountDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);
            var dashDbName = "TestDashDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);
            var convoDbName = "TestConvoDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);

            services.AddDbContext<AccountsContext>(
                opt =>
                {
                    opt.UseInMemoryDatabase(accountDbName, dbRoot);
                    SuppressWarnings(opt);
                });
            services.AddDbContext<DashContext>(
                opt =>
                {
                    opt.UseInMemoryDatabase(dashDbName, dbRoot);
                    SuppressWarnings(opt);
                });
            services.AddDbContext<ConvoContext>(
                opt =>
                {
                    opt.UseInMemoryDatabase(convoDbName, dbRoot);
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