using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class CategoryNestedThresholdResourceMapper : IMapToNew<CategoryNestedThreshold, CategoryNestedThresholdResource>
    {
        public async Task<CategoryNestedThresholdResource> Map(CategoryNestedThreshold @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new CategoryNestedThresholdResource
            {
                AccountId = @from.AccountId,
                AreaIdentifier = @from.AreaIdentifier,
                TableId = @from.TableId,
                ValueMin = @from.ValueMin,
                ValueMax = @from.ValueMax,
                Range = @from.Range,
                RowId = @from.RowId,
                RowOrder = @from.RowOrder,
                ItemId = @from.ItemId,
                ItemOrder = @from.ItemOrder,
                ItemName = @from.ItemName,
                Threshold = @from.Threshold,
                TriggerFallback = @from.TriggerFallback
            };
        }
    }
}