using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Mediator;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IReadOnlyCollection<string>> Test()// Task<IReadOnlyCollection<string>> Test()
        {
            var testData = testDataProvider.ProvideData();
            return testData;
        }
    }
}