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
    public class PutUseAreaFallbackEmailController : ControllerBase
    {
        private readonly DashContext dashContext;

        public PutUseAreaFallbackEmailController(
            DashContext dashContext 
        )
        {
            this.dashContext = dashContext;
        }

        [HttpPut("areas/{areaId}/use-fallback-email-toggle")]
        public async Task<bool> Put([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] PutUseAreaFallbackRequest request)
        {
            var area = dashContext.Areas.Where(row => row.AccountId == accountId).Single(row => row.AreaIdentifier == areaId);
            
            area.UseAreaFallbackEmail = request.UseFallback;
            await dashContext.SaveChangesAsync();
            return area.UseAreaFallbackEmail;
        }

        public class PutUseAreaFallbackRequest
        {
            public bool UseFallback { get; set; }
        }
    }
}