using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class SelectOneFlatResourceMapper : IMapToNew<CategorySelectTableRow, SelectOneFlatResource>
    {
        public async Task<SelectOneFlatResource> Map(CategorySelectTableRow @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new SelectOneFlatResource
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