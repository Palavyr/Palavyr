using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Response
{
    [Route("api")]
    [ApiController]
    public class ModifyEpilogueConfigurationController : ControllerBase
    {
        private DashContext dashContext;

        public ModifyEpilogueConfigurationController(DashContext dashContext)
        {
            this.dashContext = dashContext;
        }

        [HttpPut("response/configuration/{areaId}/epilogue")]
        public async Task<IActionResult> Modify
        (
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] EpilogueReceiver epilogueReceiver
        )
        {
            var areaRow = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);

            if (areaRow == null)
            {
                return BadRequest();
            }

            areaRow.Epilogue = epilogueReceiver.Epilogue;
            await dashContext.SaveChangesAsync();
            return Ok(epilogueReceiver.Epilogue);
        }
    }
}