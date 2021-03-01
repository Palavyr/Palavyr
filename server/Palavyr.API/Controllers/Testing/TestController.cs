using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers.Testing
{
    [Route("api")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestDataProvider testDataProvider;

        public TestController(TestDataProvider testDataProvider)
        {
            this.testDataProvider = testDataProvider;
        }

        [HttpGet("test")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 300)]
        public IReadOnlyCollection<string> Test()
        {
            var testData = testDataProvider.ProvideData();
            return testData;
        }
    }
}