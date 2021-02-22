using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Data.Abstractions;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response
{
    [Route("api")]
    [ApiController]
    public class GetResponseConfigurationController : ControllerBase
    {
        private readonly IDashConnector dashConnector;

        public GetResponseConfigurationController(IDashConnector dashConnector)
        {
            this.dashConnector = dashConnector;
        }
        
        [HttpGet("response/configuration/{areaId}")]
        public async Task<Area> Get([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var areaWithAllData = await dashConnector.GetAreaComplete(accountId, areaId);
            return areaWithAllData;
        }
    }
}