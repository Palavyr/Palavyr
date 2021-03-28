using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]

    public class PutUseAreaFallbackEmailController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;

        public PutUseAreaFallbackEmailController(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        [HttpPut("areas/{areaId}/use-fallback-email-toggle")]
        public async Task<bool> Put([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] PutUseAreaFallbackRequest request)
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            area.UseAreaFallbackEmail = request.UseFallback;
            await configurationRepository.CommitChangesAsync();
            return area.UseAreaFallbackEmail;
        }

        public class PutUseAreaFallbackRequest
        {
            public bool UseFallback { get; set; }
        }
    }
}