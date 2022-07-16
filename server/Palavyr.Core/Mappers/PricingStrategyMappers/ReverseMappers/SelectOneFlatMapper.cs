using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class SelectOneFlatMapper : IMapToNew<SelectOneFlatResource, CategorySelectTableRow>
    {
        public async Task<CategorySelectTableRow> Map(SelectOneFlatResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new CategorySelectTableRow
            {
                Id = from.Id,
                Category = from.Category,
                Range = from.Range,
                AccountId = from.AccountId,
                IntentId = from.IntentId,
                RowOrder = from.RowOrder,
                TableId = from.TableId,
                ValueMax = from.ValueMax,
                ValueMin = from.ValueMin
            };
        }
    }
}