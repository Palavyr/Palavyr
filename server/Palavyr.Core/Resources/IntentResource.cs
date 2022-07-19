using System.Collections.Generic;

namespace Palavyr.Core.Resources
{
    public class IntentResource : IEntityResource
    {
        public string IntentId { get; set; }
        public string IntentName { get; set; }
        public string Prologue { get; set; }
        public string Epilogue { get; set; }
        public string EmailTemplate { get; set; }
        public bool IsEnabled { get; set; }
        public List<StaticTableMetaResource> StaticTablesMetaResources { get; set; } = new List<StaticTableMetaResource>();
        public List<ConversationDesignerNodeResource> ConversationNodeResources { get; set; } = new List<ConversationDesignerNodeResource>();
        public List<PricingStrategyTableMetaResource> PricingStrategyTableMetaResources { get; set; } = new List<PricingStrategyTableMetaResource>();
        public string IntentSpecificEmail { get; set; }
        public bool EmailIsVerified { get; set; }
        public List<AttachmentLinkRecordResource> AttachmentRecordResources { get; set; }

        public bool UseIntentFallbackEmail { get; set; }
        public string FallbackSubject { get; set; }
        public string FallbackEmailTemplate { get; set; }
        public bool SendAttachmentsOnFallback { get; set; }
        public bool SendPdfResponse { get; set; } = true;
        public bool IncludePricingStrategyTableTotals { get; set; }

        public string Subject { get; set; }
        public int Id { get; set; }
    }
}