using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.Meta
{

    public class ModifyDynamicTableMetaController : PalavyrBaseController
    {
        private DashContext dashContext;

        public ModifyDynamicTableMetaController(DashContext dashContext)
        {
            this.dashContext = dashContext;
        }

        [HttpPut("tables/dynamic/modify")]
        public async Task<IActionResult> Modify(
            [FromBody] DynamicTableMeta dynamicTableMeta, CancellationToken cancellationToken)
        {
            if (dynamicTableMeta.Id == null)
            {
                return BadRequest();
            }

            dashContext.DynamicTableMetas.Update(dynamicTableMeta);
            await dashContext.SaveChangesAsync();
            return Ok(dynamicTableMeta);
        }
    }
}