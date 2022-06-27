using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Mappers
{
    public class PricingStrategyTableTypesResourceMapper : IMapToNew<IHaveAPrettyNameAndTableType, PricingStrategyTableTypeResource>
    {
        public async Task<PricingStrategyTableTypeResource> Map(IHaveAPrettyNameAndTableType @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new PricingStrategyTableTypeResource
            {
                PrettyName = @from.GetPrettyName(),
                TableType = @from.GetTableType()
            };
        }
    }
}