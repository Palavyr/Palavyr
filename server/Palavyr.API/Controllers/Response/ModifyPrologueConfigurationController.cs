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
    public class ModifyPrologueConfigurationController : ControllerBase
    {
        private DashContext dashContext;

        public ModifyPrologueConfigurationController(DashContext dashContext)
        {
            this.dashContext = dashContext;
        }

        [HttpPut("response/configuration/{areaId}/prologue")]
        public async Task<IActionResult> UpdatePrologue(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromBody] Prologue prologue)
        {
            var areaRow = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);
            if (areaRow == null)
            {
                return BadRequest();
            }
            
            areaRow.Prologue = prologue.Prologue_;
            await dashContext.SaveChangesAsync();
            return Ok(prologue.Prologue_);
        }
    }
}