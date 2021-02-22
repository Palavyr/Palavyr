using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Utils;
using Palavyr.Common.Utils;
using Palavyr.Data.Abstractions;
using Palavyr.Domain.Configuration.Constant;

namespace Palavyr.API.Controllers.Response.DynamicTables
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
            var allMissingNodeTypes = new List<string>();
            var area = await dashConnector.GetAreaComplete(accountId, areaId);
            var requiredDynamicNodeTypes = area
                .DynamicTableMetas
                .Select(TreeUtils.TransformRequiredNodeType)
                .ToArray();

            var rawMissingDynamicNodeTypes = TreeUtils.GetMissingNodes(area.ConversationNodes.ToArray(), requiredDynamicNodeTypes);
            var missingDynamicNodeTypes = area
                .DynamicTableMetas
                .Where(x => rawMissingDynamicNodeTypes.Contains(TreeUtils.TransformRequiredNodeType(x)))
                .Select(TreeUtils.TransformRequiredNodeTypeToPrettyName)
                .ToList();

            allMissingNodeTypes.AddRange(missingDynamicNodeTypes);

            var perIndividualRequiredStaticTables = area
                .StaticTablesMetas
                .Select(p => p.PerPersonInputRequired)
                .Any(r => r);

            if (perIndividualRequiredStaticTables && !allMissingNodeTypes.Contains(DefaultNodeTypeOptions.TakeNumberIndividuals.StringName))
            {
                var perPersonNodeType = new[] {DefaultNodeTypeOptions.TakeNumberIndividuals.StringName};
                var missingOtherNodeTypes = TreeUtils.GetMissingNodes(area.ConversationNodes.ToArray(), perPersonNodeType); //.SelectMany(x => StringUtils.SplitCamelCase(x)).ToArray();
                var asPretty = string.Join(" ", StringUtils.SplitCamelCaseAsString(missingOtherNodeTypes.First()));
                allMissingNodeTypes.Add(asPretty);
            }


            return allMissingNodeTypes.ToArray();
        }
    }
}