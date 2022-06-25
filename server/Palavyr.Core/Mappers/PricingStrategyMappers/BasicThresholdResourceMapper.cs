using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class BasicThresholdResourceMapper : IMapToNew<BasicThreshold, BasicThresholdResource>
    {
        public async Task<BasicThresholdResource> Map(BasicThreshold @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new BasicThresholdResource
            {
                Id = @from.Id,
                AccountId = @from.AccountId,
                AreaIdentifier = @from.AreaIdentifier,
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