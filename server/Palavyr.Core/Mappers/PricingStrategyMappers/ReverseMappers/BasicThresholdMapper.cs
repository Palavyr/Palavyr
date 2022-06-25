using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class BasicThresholdMapper : IMapToNew<BasicThresholdResource, BasicThreshold>
    {
        public async Task<BasicThreshold> Map(BasicThresholdResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new BasicThreshold
            {
                Id = from.Id,
                Range = from.Range,
                Threshold = from.Threshold,
                AccountId = from.AccountId,
                AreaIdentifier = from.AreaIdentifier,
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