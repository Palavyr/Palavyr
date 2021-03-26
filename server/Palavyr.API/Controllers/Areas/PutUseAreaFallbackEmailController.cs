using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]

    public class PutUseAreaFallbackEmailController : PalavyrBaseController
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