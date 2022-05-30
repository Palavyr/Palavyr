using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers
{
    public class SelectOneFlatResourceMapper : IMapToNew<SelectOneFlat, SelectOneFlatRowResource>
    {
        public async Task<SelectOneFlatRowResource> Map(SelectOneFlat @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new SelectOneFlatRowResource
            {
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