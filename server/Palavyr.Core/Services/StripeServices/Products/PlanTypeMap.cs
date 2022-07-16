using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Services.StripeServices.Products
{
    public class PlanTypeMap
    {
        public string ProductId { get; set; }
        public Account.PlanTypeEnum PlanType { get; set; }

        public PlanTypeMap(string productId, Account.PlanTypeEnum planType)
        {
            ProductId = productId;
            PlanType = planType;
        }
    }
}