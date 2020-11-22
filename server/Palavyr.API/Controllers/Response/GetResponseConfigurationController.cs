using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class GetResponseConfigurationController : ControllerBase
    {
        private DashContext dashContext;

        public GetResponseConfigurationController(DashContext dashContext)
        {
            this.dashContext = dashContext;
        }
        
        [HttpGet("response/configuration/{areaId}")]
        public async Task<IActionResult> Get([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var areaData = dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee);
            var area = await areaData.SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);
            return Ok(area);
        }
    }
}