using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public interface IDynamicTableController<TEntity, TResource> where TResource : IPricingStrategyTableRowResource, new()
    {
        Task Delete(string intentId, string tableId, CancellationToken cancellationToken);
        Task<TResource> GetDynamicRowTemplate([FromRoute] string intentId, [FromRoute] string tableId);
        Task<DynamicTableDataResource<TResource>> GetPricingStrategyTableRows([FromRoute] string intentId, [FromRoute] string tableId);


        Task<IEnumerable<TResource>> SaveDynamicTable(
            [FromRoute] string intentId, [FromRoute] string tableId,
            [FromBody] DynamicTable<TEntity> dynamicTable);

    }
}