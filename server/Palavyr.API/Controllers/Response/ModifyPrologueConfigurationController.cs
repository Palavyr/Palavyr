using System.Threading.Tasks;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Response
{
    [Route("api")]
    [ApiController]
    public class ModifyPrologueConfigurationController : ControllerBase
    {
        private readonly IDashConnector dashConnector;

        public ModifyPrologueConfigurationController(IDashConnector dashConnector)
        {
            this.dashConnector = dashConnector;
        }

        [HttpPut("response/configuration/{areaId}/prologue")]
        public async Task<string> UpdatePrologue(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromBody] PrologueReceiver prologueReceiver)
        {
            var area = await dashConnector.GetAreaById(accountId, areaId);
            area.Prologue = prologueReceiver.Prologue;
            await dashConnector.CommitChangesAsync();
            return prologueReceiver.Prologue;
        }
    }
}