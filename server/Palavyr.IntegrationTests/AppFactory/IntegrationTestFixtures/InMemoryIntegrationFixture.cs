using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Palavyr.Core.Exceptions;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Stores;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators;
using Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts;
using Test.Common;
using Xunit.Abstractions;

// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1#inject-mock-services 
namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures
{
    public abstract class NewInMemoryIntegrationFixture : BaseIntegrationFixture
    {

        protected NewInMemoryIntegrationFixture(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
            var dbRoot = new InMemoryDatabaseRoot();
            WebHostFactory = Factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureAppConfiguration(
                                (context, configBuilder) => { configBuilder.AddConfiguration(TestConfiguration.GetTestConfiguration(Assembly.GetExecutingAssembly())); })
                            .ConfigureTestContainer<ContainerBuilder>(builder => { CustomizeContainer(builder); })
                            .ConfigureInMemoryDatabase(dbRoot);
                        // .UseTestServer();
                    });
        }
    }

    public abstract class InMemoryIntegrationFixture : NewInMemoryIntegrationFixture
    {
        protected InMemoryIntegrationFixture(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
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

            var sessionStore = ResolveStore<Session>();
            var session = await sessionStore.DangerousRawQuery().SingleOrDefaultAsync(x => x.SessionId == SessionId);
            if (session is null) throw new PalavyrStartupException("Failed to set the session");
            AccountId = session.AccountId;
            
            Client.DefaultRequestHeaders.Add(ApplicationConstants.MagicUrlStrings.SessionId, SessionId);

            var unitOfWork = ResolveType<IUnitOfWorkContextProvider>();
            await unitOfWork.DangerousCommitAllContexts();
            await base.InitializeAsync();
        }
    }
}