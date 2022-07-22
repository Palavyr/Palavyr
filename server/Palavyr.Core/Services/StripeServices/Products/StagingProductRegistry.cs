
using System.Linq;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Services.StripeServices.Products
{
    public class StagingProductRegistry : IProductRegistry
    {
        public PlanTypeMap[] ProductList =>
            new[]
            {
                new PlanTypeMap(StagingProducts.FreeProduct.FreeProductId, Account.PlanTypeEnum.Free),
                new PlanTypeMap(StagingProducts.LyteProduct.LyteProductId, Account.PlanTypeEnum.Lyte),
                new PlanTypeMap(StagingProducts.PremiumProduct.PremiumProductId, Account.PlanTypeEnum.Premium),
                new PlanTypeMap(StagingProducts.ProProduct.ProProductId, Account.PlanTypeEnum.Pro)
            };

        public Account.PlanTypeEnum GetPlanTypeEnum(string productId)
        {
            var product = ProductList.SingleOrDefault(prod => prod.ProductId == productId);
            if (product == null)
            {
                throw new ProductNotRegisteredException($"The product {productId} was not registered in ProductIds.cs");
            }

            return product.PlanType;
        }

        public ProductIds GetProductIds()
        {
            return new ProductIds(
                StagingProducts.FreeProduct.FreeProductId,
                StagingProducts.LyteProduct.LyteProductId,
                StagingProducts.PremiumProduct.PremiumProductId,
                StagingProducts.ProProduct.ProProductId);
        }
    }
}