using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Data;
using Palavyr.Domain.Configuration.Schemas;

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
            [FromHeader] string accountId,
            [FromBody] DynamicTableMeta dynamicTableMeta)
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