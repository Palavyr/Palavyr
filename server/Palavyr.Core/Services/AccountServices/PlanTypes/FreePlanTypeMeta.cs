using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public class FreePlanTypeMeta : PlanTypeMeta
    {
        public FreePlanTypeMeta()
        {
            AllowedAttachments = 0;
            AllowedStaticTables = 1;
            AllowedDynamicTables = 1;
            AllowedAreas = 2;

            AllowedImageUpload = false;
            AllowedEmailNotifications = false;
            AllowedInlineEmailEditor = false;
            AllowedSmsNotifications = false;

            PlanType = Account.PlanTypeEnum.Free.ToString();
            IsFreePlan = true;
        }
    }

    public class LytePlanTypeMeta : PlanTypeMeta
    {
        public LytePlanTypeMeta()
        {
            AllowedAttachments = 0;
            AllowedStaticTables = 2;
            AllowedDynamicTables = 2;
            AllowedAreas = 6;

            AllowedImageUpload = false;
            AllowedEmailNotifications = false;
            AllowedInlineEmailEditor = false;
            AllowedSmsNotifications = false;

            PlanType = Account.PlanTypeEnum.Lyte.ToString();
            IsFreePlan = false;
        }
    }

    public class PremiumPlanTypeMeta : PlanTypeMeta
    {
        public PremiumPlanTypeMeta()
        {
            AllowedAttachments = 2;
            AllowedStaticTables = 2;
            AllowedDynamicTables = 2;
            AllowedAreas = 10;

            AllowedImageUpload = true;
            AllowedEmailNotifications = true;
            AllowedInlineEmailEditor = true;
            AllowedSmsNotifications = false;

            PlanType = Account.PlanTypeEnum.Premium.ToString();
            IsFreePlan = false;
        }
    }

    public class ProPlanTypeMeta : PlanTypeMeta
    {
        public ProPlanTypeMeta()
        {
            AllowedAttachments = 999999;
            AllowedStaticTables = 999999;
            AllowedDynamicTables = 999999;
            AllowedAreas = 999999;

            AllowedImageUpload = true;
            AllowedEmailNotifications = true;
            AllowedInlineEmailEditor = true;
            AllowedSmsNotifications = true;
            
            PlanType = Account.PlanTypeEnum.Pro.ToString();
            IsFreePlan = false;
        }
    }
}