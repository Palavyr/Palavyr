using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class TwoNestedCategoryMapper : IMapToNew<SelectWithNestedSelectResource, SelectWithNestedSelectTableRow>
    {
        private readonly IAccountIdTransport accountIdTransport;

        public TwoNestedCategoryMapper(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<SelectWithNestedSelectTableRow> Map(SelectWithNestedSelectResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new SelectWithNestedSelectTableRow
            {
                Id = from.Id,
                Range = from.Range,
                AccountId = accountIdTransport.AccountId,
                IntentId = from.IntentId,
                ItemId = from.ItemId,
                Category = from.ItemId,
                ItemOrder = from.ItemOrder,
                RowId = from.RowId,
                RowOrder = from.RowOrder,
                TableId = from.TableId,
                ValueMax = from.ValueMax,
                ValueMin = from.ValueMin,
                InnerItemName = from.InnerItemName
            };
        }
    }
}