using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class PricingStrategyTableTypesResourceMapper : IMapToNew<PricingStrategyType, PricingStrategyTableTypeResource>
    {
        public async Task<PricingStrategyTableTypeResource> Map(PricingStrategyType @from)
        {
            await Task.CompletedTask;
            return new PricingStrategyTableTypeResource
            {
                PrettyName = @from.PrettyName,
                TableType = @from.TableType
            };
        }
    }
}