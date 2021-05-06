using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    public class GetStaticTableRowTemplateController : PalavyrBaseController
    {
        public GetStaticTableRowTemplateController()
        {
        }

        [HttpGet("response/configuration/{areaId}/static/tables/{tableId}/row/template")]
        public StaticTableRow Get(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromRoute] string tableId)
        {
            return StaticTableRow.CreateStaticTableRowTemplate(int.Parse(tableId), areaId, accountId);
        }
    }
}