using System.Collections.Generic;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Resources
{
    public class IntentResource
    {
        public string IntentId { get; set; }
        public string IntentName { get; set; }
        public string Prologue { get; set; }
        public string Epilogue { get; set; }
        public string EmailTemplate { get; set; }
        public bool IsEnabled { get; set; }
        public List<StaticTablesMeta> StaticTablesMetas { get; set; } = new List<StaticTablesMeta>();
        public List<ConversationNode> ConversationNodes { get; set; } = new List<ConversationNode>();
        public List<PricingStrategyTableMeta> PricingStrategyTableMetas { get; set; } = new List<PricingStrategyTableMeta>();
        public string IntentSpecificEmail { get; set; }
        public bool EmailIsVerified { get; set; }
        public List<AttachmentLinkRecord> AttachmentRecords { get; set; }

        public bool UseIntentFallbackEmail { get; set; }
        public string FallbackSubject { get; set; } = null!;
        public string FallbackEmailTemplate { get; set; } = null!;
        public bool SendAttachmentsOnFallback { get; set; }
        public bool SendPdfResponse { get; set; } = true;
        public bool IncludePricingStrategyTableTotals { get; set; }

        public string Subject { get; set; } = null!;
    }
}