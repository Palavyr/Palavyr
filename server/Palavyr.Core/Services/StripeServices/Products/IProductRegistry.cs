using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Services.StripeServices.Products
{
    public interface IProductRegistry
    {
        Account.PlanTypeEnum GetPlanTypeEnum(string productId);
        ProductIds GetProductIds();
    }
}