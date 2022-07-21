using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class BasicThresholdMapper : IMapToNew<SimpleThresholdResource, SimpleThresholdTableRow>
    {
        public async Task<SimpleThresholdTableRow> Map(SimpleThresholdResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new SimpleThresholdTableRow
            {
                Id = from.Id,
                Range = from.Range,
                Threshold = from.Threshold,
                AccountId = from.AccountId,
                IntentId = from.IntentId,
                ItemName = from.ItemName,
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