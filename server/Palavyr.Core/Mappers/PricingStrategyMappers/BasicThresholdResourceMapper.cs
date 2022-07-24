using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class BasicThresholdResourceMapper : IMapToNew<SimpleThresholdTableRow, SimpleThresholdResource>
    {
        public async Task<SimpleThresholdResource> Map(SimpleThresholdTableRow @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new SimpleThresholdResource
            {
                Id = @from.Id.Value,
                IntentId = @from.IntentId,
                TableId = @from.TableId,
                RowId = @from.RowId,
                Threshold = @from.Threshold,
                ValueMin = @from.ValueMin,
                ValueMax = @from.ValueMax,
                Range = @from.Range,
                ItemName = @from.ItemName,
                RowOrder = @from.RowOrder,
                TriggerFallback = @from.TriggerFallback
            };
        }
    }
}