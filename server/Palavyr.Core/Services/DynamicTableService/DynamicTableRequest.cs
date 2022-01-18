using Microsoft.AspNetCore.Mvc;

namespace Palavyr.Core.Services.DynamicTableService
{
    public class DynamicTableRequest
    {
        [FromRoute(Name = "areaId")] public string AreaId { get; set; }

        [FromRoute(Name = "tableId")] public string TableId { get; set; }
        
        public void Deconstruct(out string areaIdentifier, out string tableId)
        {
            areaIdentifier = AreaId;
            tableId = TableId;
        }
    }
}