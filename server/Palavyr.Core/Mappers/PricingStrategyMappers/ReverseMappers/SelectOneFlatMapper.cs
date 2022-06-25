using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers.PricingStrategyMappers.ReverseMappers
{
    public class SelectOneFlatMapper : IMapToNew<SelectOneFlatResource, SelectOneFlat>
    {
        public async Task<SelectOneFlat> Map(SelectOneFlatResource from, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new SelectOneFlat
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