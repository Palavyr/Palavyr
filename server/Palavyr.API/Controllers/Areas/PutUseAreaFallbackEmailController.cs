using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DashboardServer.Data.Abstractions;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class PutUseAreaFallbackEmailController : ControllerBase
    {
        private readonly IDashConnector dashConnector;

        public PutUseAreaFallbackEmailController(IDashConnector dashConnector)
        {
            this.dashConnector = dashConnector;
        }

        [HttpPut("areas/{areaId}/use-fallback-email-toggle")]
        public async Task<bool> Put([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] PutUseAreaFallbackRequest request)
        {
            var area = await dashConnector.GetAreaById(accountId, areaId);
            area.UseAreaFallbackEmail = request.UseFallback;
            await dashConnector.CommitChangesAsync();
            return area.UseAreaFallbackEmail;
        }

        public class PutUseAreaFallbackRequest
        {
            public bool UseFallback { get; set; }
        }
    }
}