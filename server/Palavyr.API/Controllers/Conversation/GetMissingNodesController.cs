using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Data.Abstractions;

namespace Palavyr.API.Controllers.Conversation
{
    public class RequiredDetails
    {
        public string Type { get; set; }
        public string PrettyName { get; set; }

        public static RequiredDetails Create(string type, string prettyName)
        {
            return new RequiredDetails()
            {
                Type = type,
                PrettyName = prettyName
            };
        }
    }

    [Route("api")]
    [ApiController]
    public class GetMissingNodesController
    {
        private ILogger<GetMissingNodesController> logger;
        private readonly IDashConnector dashConnector;

        public GetMissingNodesController(
            ILogger<GetMissingNodesController> logger,
            IDashConnector dashConnector
        )
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpGet("configure-conversations/{areaId}/missing-nodes")]
        public async Task<string[]> Get([FromHeader] string accountId, string areaId)
        {
            var area = await dashConnector.GetAreaComplete(accountId, areaId);
            var allMissingNodeTypes = MissingNodeCalculator.CalculateMissingNodes(area);
            return allMissingNodeTypes.ToArray();
        }
    }
}