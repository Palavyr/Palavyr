using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class CreateAreaController : ControllerBase
    {
        private AccountsContext accountContext;
        private DashContext dashContext;
        private ILogger<CreateAreaController> logger;

        public CreateAreaController(
            AccountsContext accountContext,
            DashContext dashContext,
            ILogger<CreateAreaController> logger
        )
        {
            this.accountContext = accountContext;
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpPost("areas/create")]
        public IActionResult Create([FromHeader] string accountId, [FromBody] Text text)
        {
            var account = accountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            if (account == null)
            {
                logger.LogDebug("Account was null when it should not be");
                return BadRequest();
            }

            var defaultEmail = account.EmailAddress;
            var isVerified = account.DefaultEmailIsVerified;

            var defaultAreaTemplate = Area.CreateNewArea(text.AreaName, accountId, defaultEmail, isVerified);
            var result = dashContext.Areas.Add(defaultAreaTemplate);
            
            dashContext.SaveChanges();
            return Ok(result.Entity);
        }
    }
}