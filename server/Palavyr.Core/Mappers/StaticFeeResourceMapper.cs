using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

namespace Palavyr.Core.Mappers
{
    public class StaticFeeResourceMapper : IMapToNew<StaticFee, StaticFeeResource>
    {
        public async Task<StaticFeeResource> Map(StaticFee from, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return new StaticFeeResource
            {
                Id = @from.Id,
                Max = from.Max,
                Min = from.Min,
                FeeId = from.FeeId,
                IntentId = from.IntentId
            };
        }
    }
}