using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers.Testing
{
    public class TestController : PalavyrBaseController
    {
        private readonly TestDataProvider testDataProvider;

        public TestController(TestDataProvider testDataProvider)
        {
            this.testDataProvider = testDataProvider;
        }

        [AllowAnonymous]
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