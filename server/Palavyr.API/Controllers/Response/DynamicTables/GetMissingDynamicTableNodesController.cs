using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Palavyr.API.Utils;
using Palavyr.Common.Utils;

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

        public GetMissingDynamicTableNodesController(
            ILogger<GetMissingDynamicTableNodesController> logger,
            DashContext dashContext
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpGet("configure-conversations/{areaId}/missing-nodes")]
        public async Task<string[]> Get([FromHeader] string accountId, string areaId)
        {
            var allMissingNodeTypes = new List<string>();

            var nodelist = await dashContext
                .ConversationNodes
                .Where(row => row.AreaIdentifier == areaId)
                .ToArrayAsync();

            var area = await dashContext
                .Areas
                .Where(row => row.AreaIdentifier == areaId && row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .SingleOrDefaultAsync();

            var dynamicNodeTypes = area
                .DynamicTableMetas
                .ToArray();

            var requiredDynamicNodeTypes = dynamicNodeTypes
                .Select(TreeUtils.TransformRequiredNodeType)
                .ToList();

            var rawMissingDynamicNodeTypes = TreeUtils.GetMissingNodes(nodelist, requiredDynamicNodeTypes.ToArray());
            var missingDynamicNodeTypes = dynamicNodeTypes
                .Where(x => rawMissingDynamicNodeTypes.Contains(TreeUtils.TransformRequiredNodeType(x)))
                .Select(TreeUtils.TransformRequiredNodeTypeToPrettyName)
                .ToList();

            allMissingNodeTypes.AddRange(missingDynamicNodeTypes);

            // Other node types missing
            // var staticTableRows = await dashContext
            //     .StaticTablesRows
            //     .Where(row => row.AccountId == accountId)
            //     .ToListAsync();

            // check static tables and dynamic tables to see if even 1 'per individual' is set. If so, then check for this node type.
            // var perIndividualRequired = staticTableRows
            //     .Where(row => row.AreaIdentifier == area.AreaIdentifier)
            //     .Select(x => x.PerPerson)
            //     .Any();
            //
            // if (perIndividualRequired)
            // {
            //     var perPersonNodeType = new[] {DefaultNodeTypeOptions.TakeNumberIndividuals.StringName};
            //     var missingOtherNodeTypes = TreeUtils.GetMissingNodes(nodelist, perPersonNodeType); //.SelectMany(x => StringUtils.SplitCamelCase(x)).ToArray();
            //     var asPretty = string.Join(" ", StringUtils.SplitCamelCase(missingOtherNodeTypes.First()));
            //     allMissingNodeTypes.AddRange(new[] {asPretty});
            // }

            return allMissingNodeTypes.ToArray();
        }
    }
}