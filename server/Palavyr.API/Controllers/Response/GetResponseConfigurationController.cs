using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

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
        public async Task<Area> Get([FromRoute] string areaId)
        {
            var areaWithAllData = await configurationRepository.GetAreaComplete(areaId);
            return areaWithAllData;
        }
    }
}