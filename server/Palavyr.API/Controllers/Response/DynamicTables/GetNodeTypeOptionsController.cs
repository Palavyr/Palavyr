using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.DynamicTableService;
using Server.Domain.Configuration.Constant;

namespace Palavyr.API.Controllers.Response.DynamicTables
{
    [Route("api")]
    [ApiController]
    public class GetNodeTypeOptionsController : ControllerBase
    {
        private ILogger<GetNodeTypeOptionsController> logger;
        private DashContext dashContext;
        private ICompileDynamicTables compileDynamicTables;

        public GetNodeTypeOptionsController(
            ILogger<GetNodeTypeOptionsController> logger,
            DashContext dashContext,
            ICompileDynamicTables compileDynamicTables
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
            this.compileDynamicTables = compileDynamicTables;
        }

        [HttpGet("configure-conversations/{areaId}/node-type-options")]
        public async Task<NodeTypeOption[]> Get([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var dynamicTableMetas = await dashContext
                .DynamicTableMetas
                .Where(row => row.AccountId == accountId && row.AreaIdentifier == areaId)
                .ToArrayAsync();
            
            var dynamicTableData = await compileDynamicTables.CompileTables(dynamicTableMetas, accountId, areaId);

            var defaultNodeTypeOptions = DefaultNodeTypeOptions.DefaultNodeTypeOptionsList;
            var fullNodeTypeOptionsList = defaultNodeTypeOptions.AddAdditionalNodes(dynamicTableData);

            return fullNodeTypeOptionsList.ToArray();
        }
    }
}