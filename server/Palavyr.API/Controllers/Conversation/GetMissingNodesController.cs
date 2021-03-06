using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.DatabaseService;

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

        [HttpPost("configure-conversations/{areaId}/missing-nodes")]
        public async Task<string[]> Get([FromHeader] string accountId, string areaId, [FromBody] ConversationNodeDto currentNodes)
        {
            var area = await dashConnector.GetAreaComplete(accountId, areaId);

            var requiredDynamicNodeTypes = area
                .DynamicTableMetas
                .Select(TreeUtils.TransformRequiredNodeType)
                .ToArray();

            var dynamicTableMetas = area.DynamicTableMetas;
            var staticTableMetas = area.StaticTablesMetas;
            
            
            var allMissingNodeTypes = MissingNodeCalculator.CalculateMissingNodes(requiredDynamicNodeTypes, currentNodes.Transactions, dynamicTableMetas, staticTableMetas);
            return allMissingNodeTypes.ToArray();
        }
    }
}