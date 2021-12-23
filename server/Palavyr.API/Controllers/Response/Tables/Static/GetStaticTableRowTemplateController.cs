using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    public class GetStaticTableRowTemplateController : PalavyrBaseController
    {
        private readonly IHoldAnAccountId accountIdHolder;

        public GetStaticTableRowTemplateController(IHoldAnAccountId accountIdHolder)
        {
            this.accountIdHolder = accountIdHolder;
        }

        [HttpGet("response/configuration/{areaId}/static/tables/{tableId}/row/template")]
        public StaticTableRow Get(
            [FromRoute] string areaId,
            [FromRoute] string tableId)
        {
            return StaticTableRow.CreateStaticTableRowTemplate(int.Parse(tableId), areaId, accountIdHolder.AccountId);
        }
    }
}