using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class PercentOfThresholdResourceMapper : IMapToNew<PercentOfThresholdTableRow, PercentOfThresholdResource>
    {
        public async Task<PercentOfThresholdResource> Map(PercentOfThresholdTableRow @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new PercentOfThresholdResource
            {
                Id = @from.Id,
                AccountId = @from.AccountId,
                IntentId = @from.IntentId,
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
                ItemName = @from.Category
            };
        }
    }
}