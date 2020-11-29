using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Utils;

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
            
            var requiredNodeTypes = dynamicNodeTypes
                .Select(TreeUtils.TransformRequiredNodeType)
                .ToArray();
            
            var missingNodes = TreeUtils.GetMissingNodes(nodelist, requiredNodeTypes);
            var missingNodesPrettyNames = dynamicNodeTypes
                .Where(x => missingNodes.Contains(TreeUtils.TransformRequiredNodeType(x)))
                .Select(TreeUtils.TransformRequiredNodeTypeToPrettyName)
                .ToArray();

            return missingNodesPrettyNames;
        }
    }
}