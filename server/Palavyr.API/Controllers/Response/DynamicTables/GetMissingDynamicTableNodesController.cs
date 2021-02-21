using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Utils;
using Palavyr.Common.Utils;
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
    public class GetMissingDynamicTableNodesController
    {
        private ILogger<GetMissingDynamicTableNodesController> logger;
        private DashContext dashContext;
        private readonly IDashConnector dashConnector;

        public GetMissingDynamicTableNodesController(
            ILogger<GetMissingDynamicTableNodesController> logger,
            DashContext dashContext,
            IDashConnector dashConnector
        )
        {
            this.dashContext = dashContext;
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
                .SelectMany(x => x.StaticTableRows)
                .Select(x => x.PerPersonInputRequired)
                .Any(p => p);

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