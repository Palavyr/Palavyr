using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.StripeServices.Products;

namespace Palavyr.Core.Mappers
{
    public class ProductIdMapper : IMapToNew<ProductIds, ProductIdResource>
    {
        public async Task<ProductIdResource> Map(ProductIds @from, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new ProductIdResource
            {
                FreeProductId = @from.FreeProductId,
                LyteProductId = @from.LyteProductId,
                PremiumProductId = @from.PremiumProductId,
                ProProductId = @from.ProProductId
            };
        }
    }
}