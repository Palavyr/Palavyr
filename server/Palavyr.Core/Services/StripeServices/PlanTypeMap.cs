using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Services.StripeServices
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