using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Areas
{

    public class PutUseAreaFallbackEmailController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;

        public PutUseAreaFallbackEmailController(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        [HttpPut("areas/{areaId}/use-fallback-email-toggle")]
        public async Task<bool> Put([FromRoute] string areaId, [FromBody] PutUseAreaFallbackRequest request)
        {
            var area = await configurationRepository.GetAreaById(areaId);
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