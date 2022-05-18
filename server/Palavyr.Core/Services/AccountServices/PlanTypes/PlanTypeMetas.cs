using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public class FreePlanTypeMeta : PlanTypeMetaBase
    {
        public FreePlanTypeMeta()
        {
            AllowedAttachments = 0;
            AllowedStaticTables = 1;
            AllowedDynamicTables = 1;
            AllowedAreas = 2;

            AllowedFileUpload = false;
            AllowedEmailNotifications = false;
            AllowedInlineEmailEditor = false;
            AllowedSmsNotifications = false;

            PlanType = Account.PlanTypeEnum.Free.ToString();
            IsFreePlan = true;
        }
    }

    public class LytePlanTypeMeta : PlanTypeMetaBase
    {
        public LytePlanTypeMeta()
        {
            AllowedAttachments = 0;
            AllowedStaticTables = 2;
            AllowedDynamicTables = 2;
            AllowedAreas = 6;

            AllowedFileUpload = false;
            AllowedEmailNotifications = false;
            AllowedInlineEmailEditor = false;
            AllowedSmsNotifications = false;

            PlanType = Account.PlanTypeEnum.Lyte.ToString();
            IsFreePlan = false;
        }
    }

    public class PremiumPlanTypeMeta : PlanTypeMetaBase
    {
        public PremiumPlanTypeMeta()
        {
            AllowedAttachments = 2;
            AllowedStaticTables = 2;
            AllowedDynamicTables = 2;
            AllowedAreas = 10;

            AllowedFileUpload = true;
            AllowedEmailNotifications = true;
            AllowedInlineEmailEditor = true;
            AllowedSmsNotifications = false;

            PlanType = Account.PlanTypeEnum.Premium.ToString();
            IsFreePlan = false;
        }
    }

    public class ProPlanTypeMeta : PlanTypeMetaBase
    {
        public ProPlanTypeMeta()
        {
            AllowedAttachments = 999999;
            AllowedStaticTables = 999999;
            AllowedDynamicTables = 999999;
            AllowedAreas = 999999;

            AllowedFileUpload = true;
            AllowedEmailNotifications = true;
            AllowedInlineEmailEditor = true;
            AllowedSmsNotifications = true;

            PlanType = Account.PlanTypeEnum.Pro.ToString();
            IsFreePlan = false;
        }
    }
}