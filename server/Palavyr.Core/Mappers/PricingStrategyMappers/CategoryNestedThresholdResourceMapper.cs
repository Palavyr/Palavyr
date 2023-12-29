using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class CategoryNestedThresholdResourceMapper : IMapToNew<CategoryNestedThresholdTableRow, CategoryNestedThresholdResource>
    {
        public async Task<CategoryNestedThresholdResource> Map(CategoryNestedThresholdTableRow @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new CategoryNestedThresholdResource
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
                Threshold = @from.Threshold,
                TriggerFallback = @from.TriggerFallback
            };
        }
    }
}