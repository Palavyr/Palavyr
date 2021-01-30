using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class PutAreaIsCompleteController : ControllerBase
    {
        private readonly DashContext dashContext;

        public PutAreaIsCompleteController(
            DashContext dashContext 
        )
        {
            this.dashContext = dashContext;
        }

        [HttpPut("areas/{areaId}/area-toggle")]
        public async Task<bool> Put([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] PutAreaIsCompleteRequest request)
        {
            var area = dashContext.Areas.Where(row => row.AccountId == accountId).Single(row => row.AreaIdentifier == areaId);
            area.IsComplete = request.IsComplete;
            await dashContext.SaveChangesAsync();
            return area.IsComplete;
        }

        public class PutAreaIsCompleteRequest
        {
            public bool IsComplete { get; set; }
        }
    }
}