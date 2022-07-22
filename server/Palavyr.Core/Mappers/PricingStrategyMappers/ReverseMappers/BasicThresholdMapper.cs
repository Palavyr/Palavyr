using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class BasicThresholdMapper : IMapToNew<SimpleThresholdResource, SimpleThresholdTableRow>
    {
        private readonly IAccountIdTransport accountIdTransport;

        public BasicThresholdMapper(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }
        public async Task<SimpleThresholdTableRow> Map(SimpleThresholdResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new SimpleThresholdTableRow
            {
                Id = from.Id,
                Range = from.Range,
                Threshold = from.Threshold,
                AccountId = accountIdTransport.AccountId,
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