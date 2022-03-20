using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public class FreePlanTypeMetaBase : PlanTypeMetaBase
    {
        public FreePlanTypeMetaBase()
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

    public class LytePlanTypeMetaBase : PlanTypeMetaBase
    {
        public LytePlanTypeMetaBase()
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

    public class PremiumPlanTypeMetaBase : PlanTypeMetaBase
    {
        public PremiumPlanTypeMetaBase()
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

    public class ProPlanTypeMetaBase : PlanTypeMetaBase
    {
        public ProPlanTypeMetaBase()
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