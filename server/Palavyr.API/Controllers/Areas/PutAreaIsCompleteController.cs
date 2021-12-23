using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]

    public class PutAreaIsCompleteController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;

        public PutAreaIsCompleteController(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        [HttpPut("areas/{areaId}/area-toggle")]
        public async Task<bool> Put([FromRoute] string areaId, [FromBody] PutAreaIsCompleteRequest request)
        {
            var area = await configurationRepository.GetAreaById(areaId);
            area.IsEnabled = request.IsEnabled;
            await configurationRepository.CommitChangesAsync();
            return area.IsEnabled;
        }

        public class PutAreaIsCompleteRequest
        {
            public bool IsEnabled { get; set; }
        }
    }
}