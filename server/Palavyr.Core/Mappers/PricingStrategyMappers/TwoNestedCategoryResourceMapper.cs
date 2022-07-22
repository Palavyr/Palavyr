using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class TwoNestedCategoryResourceMapper : IMapToNew<SelectWithNestedSelectTableRow, SelectWithNestedSelectResource>
    {
        public async Task<SelectWithNestedSelectResource> Map(SelectWithNestedSelectTableRow @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new SelectWithNestedSelectResource
            {
                Id = @from.Id,
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