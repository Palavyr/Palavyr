using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Exceptions;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Stores;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators;
using Xunit.Abstractions;

// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1#inject-mock-services 
namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures
{
    public abstract class IntegrationTest : IntegrationTest<DbTypes.Real>
    {
        protected IntegrationTest(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
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