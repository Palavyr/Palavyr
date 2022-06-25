using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class CategoryNestedThresholdMapper : IMapToNew<CategoryNestedThresholdResource, CategoryNestedThreshold>
    {
        public async Task<CategoryNestedThreshold> Map(CategoryNestedThresholdResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new CategoryNestedThreshold
            {
                Id = from.Id,
                Range = from.Range,
                Threshold = from.Threshold,
                AccountId = from.AccountId,
                AreaIdentifier = from.AreaIdentifier,
                ItemId = from.ItemId,
                ItemName = from.ItemName,
                ItemOrder = from.ItemOrder,
                RowId = from.RowId,
                RowOrder = from.RowOrder,
                TableId = from.TableId,
                TriggerFallback = from.TriggerFallback,
                ValueMax = from.ValueMax,
                ValueMin = from.ValueMin
            };
        }
    }
}