using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class SelectOneFlatMapper : IMapToNew<SelectOneFlatResource, SimpleSelectTableRow>
    {
        public async Task<SimpleSelectTableRow> Map(SelectOneFlatResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new SimpleSelectTableRow
            {
                Id = from.Id,
                Option = from.Option,
                Range = from.Range,
                AccountId = from.AccountId,
                AreaIdentifier = from.AreaIdentifier,
                RowOrder = from.RowOrder,
                TableId = from.TableId,
                ValueMax = from.ValueMax,
                ValueMin = from.ValueMin
            };
        }
    }
}