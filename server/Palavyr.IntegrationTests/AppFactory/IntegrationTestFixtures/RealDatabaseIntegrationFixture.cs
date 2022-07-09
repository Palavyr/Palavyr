using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Palavyr.Client;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Stores;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators;
using Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts;
using Test.Common;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures
{
    public abstract class NewRealDatabaseIntegrationFixture : BaseIntegrationFixture
    {
        protected NewRealDatabaseIntegrationFixture(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
            WebHostFactory = Factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureAppConfiguration(
                                (context, configBuilder) => { configBuilder.AddConfiguration(TestConfiguration.GetTestConfiguration(Assembly.GetExecutingAssembly())); })
                            .ConfigureTestContainer<ContainerBuilder>(builder => CustomizeContainer(builder))
                            .ConfigureAndCreateRealTestDatabase();
                        // .UseTestServer();
                    });
        }
        
        
    }

    public abstract class RealDatabaseIntegrationFixture : NewRealDatabaseIntegrationFixture
    {
        protected RealDatabaseIntegrationFixture(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            // override the email
            builder.RegisterType<MockEmailVerificationService>().As<IEmailVerificationService>();
            return base.CustomizeContainer(builder);
        }

        public override async Task InitializeAsync()
        {
            var credentials = await this.CreateDefaultTestAccountBuilder().Build(EmailAddress, Password);
            SessionId = credentials.SessionId;
            ApiKey = credentials.ApiKey;

            Client.DefaultRequestHeaders.Add(ApplicationConstants.MagicUrlStrings.SessionId, SessionId);

            
            var unitOfWork = ResolveType<IUnitOfWorkContextProvider>();
            await unitOfWork.DangerousCommitAllContexts();
            await base.InitializeAsync();
        }
    }
}