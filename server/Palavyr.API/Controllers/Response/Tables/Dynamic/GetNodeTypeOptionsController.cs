using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Services.DatabaseService;
using Palavyr.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class GetNodeTypeOptionsController : PalavyrBaseController
    {
        private ILogger<GetNodeTypeOptionsController> logger;
        private readonly IDashConnector dashConnector;
        private ICompileDynamicTables compileDynamicTables;

        public GetNodeTypeOptionsController(
            ILogger<GetNodeTypeOptionsController> logger,
            IDashConnector dashConnector,
            ICompileDynamicTables compileDynamicTables
        )
        {
            this.logger = logger;
            this.dashConnector = dashConnector;
            this.compileDynamicTables = compileDynamicTables;
        }

        [HttpGet("configure-conversations/{areaId}/node-type-options")]
        public async Task<NodeTypeOption[]> Get([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var dynamicTableMetas = await dashConnector.GetDynamicTableMetas(accountId, areaId);
            var dynamicTableData = await compileDynamicTables.CompileTables(dynamicTableMetas, accountId, areaId);
            var defaultNodeTypeOptions = DefaultNodeTypeOptions.DefaultNodeTypeOptionsList;

            var fullNodeTypeOptionsList = defaultNodeTypeOptions.AddAdditionalNodes(dynamicTableData);

            return fullNodeTypeOptionsList.ToArray();
        }

        // todo : ATTEMPTED to send back options list where used dynamic node was removed, but it will destroy the currently used
        // since it also depends on the list.
        // Filtering has to be done on the frontend.
    }
}