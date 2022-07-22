using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class CategoryNestedThresholdMapper : IMapToNew<CategoryNestedThresholdResource, CategoryNestedThresholdTableRow>
    {
        private readonly IAccountIdTransport accountIdTransport;

        public CategoryNestedThresholdMapper(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<CategoryNestedThresholdTableRow> Map(CategoryNestedThresholdResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new CategoryNestedThresholdTableRow
            {
                Id = from.Id,
                Range = from.Range,
                Threshold = from.Threshold,
                AccountId = accountIdTransport.AccountId,
                IntentId = from.IntentId,
                ItemId = from.ItemId,
                Category = from.ItemName,
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