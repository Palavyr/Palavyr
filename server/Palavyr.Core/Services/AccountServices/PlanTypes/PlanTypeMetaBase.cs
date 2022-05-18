namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public abstract class PlanTypeMetaBase
    {
        public int AllowedAttachments { get; set; }
        public int AllowedStaticTables { get; set; }
        public int AllowedDynamicTables { get; set; }
        public int AllowedAreas { get; set; }

        public bool AllowedFileUpload { get; set; }
        public bool AllowedEmailNotifications { get; set; }
        public bool AllowedInlineEmailEditor { get; set; }
        public bool AllowedSmsNotifications { get; set; }

        public string PlanType { get; set; }
        public bool IsFreePlan { get; set; }

        public int GetDefaultNumAreas() => (new LytePlanTypeMeta()).AllowedAreas;
    }
}