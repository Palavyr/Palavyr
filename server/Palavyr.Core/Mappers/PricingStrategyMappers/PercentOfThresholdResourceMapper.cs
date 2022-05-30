using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class PercentOfThresholdResourceMapper : IMapToNew<PercentOfThreshold, PercentOfThresholdResource>
    {
        public async Task<PercentOfThresholdResource> Map(PercentOfThreshold @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new PercentOfThresholdResource
            {
                AccountId = @from.AccountId,
                AreaIdentifier = @from.AreaIdentifier,
                TableId = @from.TableId,
                RowId = @from.RowId,
                Threshold = @from.Threshold,
                ValueMin = @from.ValueMin,
                ValueMax = @from.ValueMax,
                Range = @from.Range,
                Modifier = @from.Modifier,
                PosNeg = @from.PosNeg,
                RowOrder = @from.RowOrder,
                TriggerFallback = @from.TriggerFallback,
                ItemOrder = @from.ItemOrder,
                ItemId = @from.ItemId,
                ItemName = @from.ItemName
            };
        }
    }
}