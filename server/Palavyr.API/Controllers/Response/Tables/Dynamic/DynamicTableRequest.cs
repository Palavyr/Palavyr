using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class DynamicTableRequest
    {
        [FromRoute(Name = "areaId")] public string AreaId { get; set; }

        [FromRoute(Name = "tableId")] public string TableId { get; set; }

        [FromHeader(Name = "accountId")] public string AccountId { get; set; }

        public void Deconstruct(out string accountId, out string areaIdentifier, out string tableId)
        {
            areaIdentifier = AreaId;
            tableId = TableId;
            accountId = AccountId;
        }
    }
}