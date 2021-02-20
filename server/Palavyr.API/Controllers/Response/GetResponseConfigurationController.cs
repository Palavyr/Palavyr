using System.Threading.Tasks;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
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
            var areaWithAllData = await dashConnector.GetAreaDeep(accountId, areaId);
            return areaWithAllData;
        }
    }
}