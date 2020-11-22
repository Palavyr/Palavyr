using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.API.ReceiverTypes;

namespace Palavyr.API.Controllers
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
            [FromBody] Epilogue epilogue
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

            areaRow.Epilogue = epilogue.Epilogue_;
            await dashContext.SaveChangesAsync();
            return Ok(epilogue.Epilogue_);
        }
    }
}