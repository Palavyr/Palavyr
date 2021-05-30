#nullable enable
using System.Linq;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IProductRegistry
    {
        Account.PlanTypeEnum GetPlanTypeEnum(string productId);
        ProductIds GetProductIds();
    }

    public class ProductRegistry : IProductRegistry
    {
        public PlanTypeMap[] ProductList =>
            new[]
            {
                new PlanTypeMap(Products.FreeProduct.FreeProductId, Account.PlanTypeEnum.Free),
                new PlanTypeMap(Products.LyteProduct.LyteProductId, Account.PlanTypeEnum.Lyte),
                new PlanTypeMap(Products.PremiumProduct.PremiumProductId, Account.PlanTypeEnum.Premium),
                new PlanTypeMap(Products.ProProduct.ProProductId, Account.PlanTypeEnum.Pro)
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
                Products.FreeProduct.FreeProductId,
                Products.LyteProduct.LyteProductId,
                Products.PremiumProduct.PremiumProductId,
                Products.ProProduct.ProProductId);
        }
    }
}