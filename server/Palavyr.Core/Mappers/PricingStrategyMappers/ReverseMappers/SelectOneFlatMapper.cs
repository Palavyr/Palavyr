using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class SelectOneFlatMapper : IMapToNew<CategorySelectTableRowResource, CategorySelectTableRow>
    {
        private readonly IAccountIdTransport accountIdTransport;

        public SelectOneFlatMapper(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<CategorySelectTableRow> Map(CategorySelectTableRowResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new CategorySelectTableRow
            {
                Id = from.Id,
                Category = from.Category,
                Range = from.Range,
                AccountId = accountIdTransport.AccountId,
                IntentId = from.IntentId,
                RowOrder = from.RowOrder,
                TableId = from.TableId,
                ValueMax = from.ValueMax,
                ValueMin = from.ValueMin
            };
        }
    }
}