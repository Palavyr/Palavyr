#nullable enable
using System.Linq;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Services.StripeServices.Products
{
    public class ProductionProductRegistry : IProductRegistry
    {
        public PlanTypeMap[] ProductList =>
            new[]
            {
                new PlanTypeMap(ProductionProducts.FreeProduct.FreeProductId, Account.PlanTypeEnum.Free),
                new PlanTypeMap(ProductionProducts.LyteProduct.LyteProductId, Account.PlanTypeEnum.Lyte),
                new PlanTypeMap(ProductionProducts.PremiumProduct.PremiumProductId, Account.PlanTypeEnum.Premium),
                new PlanTypeMap(ProductionProducts.ProProduct.ProProductId, Account.PlanTypeEnum.Pro)
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
                ProductionProducts.FreeProduct.FreeProductId,
                ProductionProducts.LyteProduct.LyteProductId,
                ProductionProducts.PremiumProduct.PremiumProductId,
                ProductionProducts.ProProduct.ProProductId);
        }
    }
}