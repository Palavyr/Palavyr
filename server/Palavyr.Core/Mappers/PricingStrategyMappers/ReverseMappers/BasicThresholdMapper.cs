using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class BasicThresholdMapper : IMapToNew<BasicThresholdResource, SimpleThresholdTableRow>
    {
        public async Task<SimpleThresholdTableRow> Map(BasicThresholdResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new SimpleThresholdTableRow
            {
                Id = from.Id,
                Range = from.Range,
                Threshold = from.Threshold,
                AccountId = from.AccountId,
                IntentId = from.AreaIdentifier,
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