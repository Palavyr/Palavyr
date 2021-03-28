using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Response
{
    public class GetResponseConfigurationController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;

        public GetResponseConfigurationController(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }
        
        [HttpGet("response/configuration/{areaId}")]
        public async Task<Area> Get([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var areaWithAllData = await configurationRepository.GetAreaComplete(accountId, areaId);
            return areaWithAllData;
        }
    }
}