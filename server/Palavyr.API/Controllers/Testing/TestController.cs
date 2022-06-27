using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers.Testing
{
    [AllowAnonymous]
    public class TestController : PalavyrBaseController
    {
        private readonly TestDataProvider testDataProvider;

        public TestController()
        {
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
    }

    public class TestRequest
    {
        public const string Route = "test";
    }
}