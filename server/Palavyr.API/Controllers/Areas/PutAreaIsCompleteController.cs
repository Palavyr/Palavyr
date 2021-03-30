using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Services.Repositories;

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
        public async Task<bool> Put([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] PutAreaIsCompleteRequest request)
        {
            var area = await configurationRepository.GetAreaById(accountId, areaId);
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