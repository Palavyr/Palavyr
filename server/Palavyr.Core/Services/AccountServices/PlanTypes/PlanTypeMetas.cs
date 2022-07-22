using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public class FreePlanTypeMeta : PlanTypeMetaResource
    {
        public FreePlanTypeMeta()
        {
            AllowedAttachments = 0;
            AllowedStaticTables = 1;
            AllowedPricingStrategyTables = 1;
            AllowedIntents = 2;

            AllowedFileUpload = false;
            AllowedEmailNotifications = false;
            AllowedInlineEmailEditor = false;
            AllowedSmsNotifications = false;

            PlanType = Account.PlanTypeEnum.Free.ToString();
            IsFreePlan = true;
        }
    }

    public class LytePlanTypeMeta : PlanTypeMetaResource
    {
        public LytePlanTypeMeta()
        {
            AllowedAttachments = 0;
            AllowedStaticTables = 2;
            AllowedPricingStrategyTables = 2;
            AllowedIntents = 6;

            AllowedFileUpload = false;
            AllowedEmailNotifications = false;
            AllowedInlineEmailEditor = false;
            AllowedSmsNotifications = false;

            PlanType = Account.PlanTypeEnum.Lyte.ToString();
            IsFreePlan = false;
        }
    }

    public class PremiumPlanTypeMeta : PlanTypeMetaResource
    {
        public PremiumPlanTypeMeta()
        {
            AllowedAttachments = 2;
            AllowedStaticTables = 2;
            AllowedPricingStrategyTables = 2;
            AllowedIntents = 10;

            AllowedFileUpload = true;
            AllowedEmailNotifications = true;
            AllowedInlineEmailEditor = true;
            AllowedSmsNotifications = false;

            PlanType = Account.PlanTypeEnum.Premium.ToString();
            IsFreePlan = false;
        }
    }

    public class ProPlanTypeMeta : PlanTypeMetaResource
    {
        public ProPlanTypeMeta()
        {
            AllowedAttachments = 999999;
            AllowedStaticTables = 999999;
            AllowedPricingStrategyTables = 999999;
            AllowedIntents = 999999;

            AllowedFileUpload = true;
            AllowedEmailNotifications = true;
            AllowedInlineEmailEditor = true;
            AllowedSmsNotifications = true;

            PlanType = Account.PlanTypeEnum.Pro.ToString();
            IsFreePlan = false;
        }
    }
}