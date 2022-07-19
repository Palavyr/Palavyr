using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class SelectOneFlatResourceMapper : IMapToNew<CategorySelectTableRow, CategorySelectTableRowResource>
    {
        public async Task<CategorySelectTableRowResource> Map(CategorySelectTableRow @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new CategorySelectTableRowResource
            {
                Id = @from.Id,
                AccountId = @from.AccountId,
                IntentId = @from.IntentId,
                TableId = @from.TableId,
                Category = @from.Category,
                ValueMin = @from.ValueMin,
                ValueMax = @from.ValueMax,
                Range = @from.Range,
                RowOrder = @from.RowOrder
            };
        }
    }
}