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

        public TestController(TestDataProvider testDataProvider)
        {
            this.testDataProvider = testDataProvider;
        }

        [HttpGet("test")]
        [ResponseCache(Duration = 300)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IReadOnlyCollection<string>> Test()
        {
            var testData = testDataProvider.ProvideData();
            return testData;
        }
    }
}