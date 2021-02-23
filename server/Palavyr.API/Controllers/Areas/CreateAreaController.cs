using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class CreateAreaController : ControllerBase
    {
        private readonly IDashConnector dashConnector;
        private readonly IAccountsConnector accountsConnector;
        private ILogger<CreateAreaController> logger;

        public CreateAreaController(

            IDashConnector dashConnector,
            IAccountsConnector accountsConnector,
            ILogger<CreateAreaController> logger
        )
        {
            this.dashConnector = dashConnector;
            this.accountsConnector = accountsConnector;
            this.logger = logger;
        }

        [HttpPost("areas/create")]
        public async Task<Area> Create([FromHeader] string accountId, [FromBody] AreaNameText areaNameText)
        {
            var account = await accountsConnector.GetAccount(accountId);

            var defaultEmail = account.EmailAddress;
            var isVerified = account.DefaultEmailIsVerified;

            var newArea = await dashConnector.CreateAndAddNewArea(areaNameText.AreaName, accountId, defaultEmail, isVerified);
            await dashConnector.CommitChangesAsync();
            return newArea;
        }
    }
}