using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data.Abstractions;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class PutAreaIsCompleteController : ControllerBase
    {
        private readonly IDashConnector dashConnector;

        public PutAreaIsCompleteController(IDashConnector dashConnector)
        {
            this.dashConnector = dashConnector;
        }

        [HttpPut("areas/{areaId}/area-toggle")]
        public async Task<bool> Put([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] PutAreaIsCompleteRequest request)
        {
            var area = await dashConnector.GetAreaById(accountId, areaId);
            area.IsComplete = request.IsComplete;
            await dashConnector.CommitChangesAsync();
            return area.IsComplete;
        }

        public class PutAreaIsCompleteRequest
        {
            public bool IsComplete { get; set; }
        }
    }
}