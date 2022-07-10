using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public interface IDynamicTableController<TEntity, TResource> where TResource : class, IPricingStrategyTableRowResource, new()
    {
        Task Delete(string intentId, string tableId, CancellationToken cancellationToken);
        Task<TResource> GetRowTemplate([FromRoute] string intentId, [FromRoute] string tableId, CancellationToken cancellationToken);
        Task<PricingStrategyTableDataResource<TResource>> GetTable([FromRoute] string intentId, [FromRoute] string tableId, CancellationToken cancellationToken);

        Task<PricingStrategyTableDataResource<TResource>> SaveTable(
            [FromRoute]
            string intentId,
            [FromRoute]
            string tableId,
            [FromBody]
            PricingStrategyTableDataResource<TResource> pricingStrategyTable,
            CancellationToken cancellationToken);
    }
}