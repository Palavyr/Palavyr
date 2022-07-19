using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class TwoNestedCategoryResourceMapper : IMapToNew<TwoNestedSelectTableRow, TwoNestedCategoryResource>
    {
        public async Task<TwoNestedCategoryResource> Map(TwoNestedSelectTableRow @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new TwoNestedCategoryResource
            {
                Id = @from.Id,
                AccountId = @from.AccountId,
                IntentId = @from.IntentId,
                TableId = @from.TableId,
                ValueMin = @from.ValueMin,
                ValueMax = @from.ValueMax,
                Range = @from.Range,
                RowId = @from.RowId,
                RowOrder = @from.RowOrder,
                ItemId = @from.ItemId,
                ItemOrder = @from.ItemOrder,
                ItemName = @from.Category,
                InnerItemName = @from.InnerItemName
            };
        }
    }
}