using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class SelectOneFlatResourceMapper : IMapToNew<SimpleSelectTableRow, SelectOneFlatResource>
    {
        public async Task<SelectOneFlatResource> Map(SimpleSelectTableRow @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new SelectOneFlatResource
            {
                Id = @from.Id,
                AccountId = @from.AccountId,
                AreaIdentifier = @from.AreaIdentifier,
                TableId = @from.TableId,
                Option = @from.Option,
                ValueMin = @from.ValueMin,
                ValueMax = @from.ValueMax,
                Range = @from.Range,
                RowOrder = @from.RowOrder
            };
        }
    }
}