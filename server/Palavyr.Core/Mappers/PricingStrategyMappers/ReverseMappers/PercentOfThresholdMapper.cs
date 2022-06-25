using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class PercentOfThresholdMapper : IMapToNew<PercentOfThresholdResource, PercentOfThreshold>
    {
        public async Task<PercentOfThreshold> Map(PercentOfThresholdResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new PercentOfThreshold
            {
                Id = from.Id,
                Modifier = from.Modifier,
                Range = from.Range,
                Threshold = from.Threshold,
                AccountId = from.AccountId,
                AreaIdentifier = from.AreaIdentifier,
                ItemId = from.ItemId,
                ItemName = from.ItemName,
                ItemOrder = from.ItemOrder,
                PosNeg = from.PosNeg,
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