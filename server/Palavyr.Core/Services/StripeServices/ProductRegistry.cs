#nullable enable
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Services.StripeServices
{
    public static class ProductRegistry
    {
        public static List<StripeProduct> Products { get; } = new List<StripeProduct>()
        {
            new StripeProduct("Free", null, null, null, Account.PlanTypeEnum.Free),
            new StripeProduct("Premium", "prod_IsrPiS05a06E9W", "price_1IH5gvAnPqY603aZ5gDHhRd6", "price_1IH5gvAnPqY603aZPsfJKhRp", Account.PlanTypeEnum.Premium),
            new StripeProduct("Pro", "prod_IsrQWp3FtLVLlL", "price_1IH5huAnPqY603aZ8krz9tDn", "price_1IH5huAnPqY603aZ6RHUE2uD", Account.PlanTypeEnum.Pro),
        };

        public static Account.PlanTypeEnum GetPlanTypeEnum(string productId)
        {
            var product = Products.Single(prod => prod.productId == productId);
            return product.planTypeEnum;
        }

        public static ProductIds GetProductIds()
        {
            return new ProductIds()
            {
                FreeProductId = Products.Single(p => p.planTypeEnum == Account.PlanTypeEnum.Free).productId,
                PremiumProductId = Products.Single(p => p.planTypeEnum == Account.PlanTypeEnum.Premium).productId,
                ProProductId = Products.Single(p => p.planTypeEnum == Account.PlanTypeEnum.Pro).productId
            };
        }
    }
    
    public class StripeProduct
    {
        public readonly string productType;
        public readonly string? productId;
        public readonly string? monthly;
        public readonly string? yearly;
        public readonly Account.PlanTypeEnum planTypeEnum;

        public StripeProduct(string productType, string? productId, string? monthly, string? yearly, Account.PlanTypeEnum planTypeEnum)
        {
            this.productType = productType;
            this.productId = productId;
            this.monthly = monthly;
            this.yearly = yearly;
            this.planTypeEnum = planTypeEnum;
        }
    }

    public class ProductIds
    {
        public string PremiumProductId { get; set; }
        public string ProProductId { get; set; }
        public string FreeProductId { get; set; }
    }
}