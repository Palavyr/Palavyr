using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class DynamicTableRequest
    {
        [FromRoute] public string areaId { get; set; }

        [FromRoute] public string tableId { get; set; }

        [FromHeader] public string accountId { get; set; }

        public void Deconstruct(out string accountId, out string areaIdentifier, out string tableId)
        {
            areaIdentifier = this.areaId;
            tableId = this.tableId;
            accountId = this.accountId;
        }
    }
}