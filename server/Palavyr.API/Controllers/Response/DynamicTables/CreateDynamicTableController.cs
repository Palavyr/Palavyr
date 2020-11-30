using System;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.FileSystem.UniqueIdentifiers;
using Server.Domain.Configuration.Constant;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.DynamicTables
{
    [Route("api")]
    [ApiController]
    public class CreateDynamicTableController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<CreateDynamicTableController> logger;

        public CreateDynamicTableController(DashContext dashContext, ILogger<CreateDynamicTableController> logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        // One controller for getting each table type. A separate call to get the type. Each table has a different
        // structure, and thus a different type. So we can't return multiple types from the same controller without
        // further generalization. This can be done later if its worth it. Adding a new controller for each type
        // isn't that big of a deal since we'll only have dozens of types probably. If we make money, then we can switch
        // to a generic pattern. Its just too complex to implement right now.
        
        [HttpPost("tables/dynamic/{areaId}")]
        public async Task<IActionResult> Create(
            [FromHeader] string accountId, 
            [FromRoute] string areaId)
        {
            var area = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);

            var dynamicTables = area.DynamicTableMetas.ToList();

            var tableId = Guid.NewGuid().ToString();
            var tableTag = GuidUtils.CreatePseudoRandomString(5);

            var newTableMeta = DynamicTableMeta.CreateNew(
                tableTag,
                DynamicTableTypes.DefaultTable.PrettyName,
                DynamicTableTypes.DefaultTable.TableType,
                tableId,
                areaId,
                accountId);

            dynamicTables.Add(newTableMeta);
            area.DynamicTableMetas = dynamicTables;
            var defaultTable = SelectOneFlat.CreateTemplate(accountId, areaId, tableId);
            await dashContext.SelectOneFlats.AddAsync(defaultTable);
            await dashContext.SaveChangesAsync();

            return Ok(newTableMeta);
        }
    }
}