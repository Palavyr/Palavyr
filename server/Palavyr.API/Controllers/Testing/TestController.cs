using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.API.Controllers.Testing
{
    [AllowAnonymous]
    public class TestController : PalavyrBaseController
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly TestDataProvider testDataProvider;

        public TestController(IEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
            this.testDataProvider = new TestDataProvider();
        }

        [HttpGet(TestRequest.Route)]
        [ResponseCache(Duration = 300)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IReadOnlyCollection<string>> Test()
        {
            await Task.CompletedTask;
            var testData = testDataProvider.ProvideData();
            return testData;
        }

        [HttpGet("test/tester")]
        public async Task<IReadOnlyCollection<string>> Tester()
        {
            await Task.CompletedTask;
            await accountStore.GetAll();
            var testData = testDataProvider.ProvideData();
            return testData;
        }
    }

    public class TestRequest : IRequest<object>
    {
        public const string Route = "test";
    }
}