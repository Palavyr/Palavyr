﻿namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public abstract class PlanTypeMetaResource
    {
        public int AllowedAttachments { get; set; }
        public int AllowedStaticTables { get; set; }
        public int AllowedPricingStrategyTables { get; set; }
        public int AllowedIntents { get; set; }

        public bool AllowedFileUpload { get; set; }
        public bool AllowedEmailNotifications { get; set; }
        public bool AllowedInlineEmailEditor { get; set; }
        public bool AllowedSmsNotifications { get; set; }

        public string PlanType { get; set; }
        public bool IsFreePlan { get; set; }

        public int GetDefaultNumIntents() => (new LytePlanTypeMeta()).AllowedIntents;
    }
}