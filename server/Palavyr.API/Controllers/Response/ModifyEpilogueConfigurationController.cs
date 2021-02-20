using System.Threading.Tasks;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Response
{
    [Route("api")]
    [ApiController]
    public class ModifyEpilogueConfigurationController : ControllerBase
    {
        private readonly IDashConnector dashConnector;

        public ModifyEpilogueConfigurationController(IDashConnector dashConnector)
        {
            this.dashConnector = dashConnector;
        }

        [HttpPut("response/configuration/{areaId}/epilogue")]
        public async Task<string> Modify
        (
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] EpilogueReceiver epilogueReceiver
        )
        {
            var area = await dashConnector.GetAreaById(accountId, areaId);
            area.Epilogue = epilogueReceiver.Epilogue;
            await dashConnector.CommitChangesAsync();
            return epilogueReceiver.Epilogue;
        }
    }
}