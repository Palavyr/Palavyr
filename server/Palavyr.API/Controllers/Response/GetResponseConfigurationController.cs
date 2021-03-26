using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.Response
{
    public class GetResponseConfigurationController : PalavyrBaseController
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