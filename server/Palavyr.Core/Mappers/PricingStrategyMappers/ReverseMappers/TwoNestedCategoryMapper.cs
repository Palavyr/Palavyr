using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class TwoNestedCategoryMapper : IMapToNew<TwoNestedCategoryResource, TwoNestedSelectTableRow>
    {
        public async Task<TwoNestedSelectTableRow> Map(TwoNestedCategoryResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new TwoNestedSelectTableRow
            {
                Id = from.Id,
                Range = from.Range,
                AccountId = from.AccountId,
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