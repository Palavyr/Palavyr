using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Resources
{
    
    
    public class IntentResource
    {
        public string AreaIdentifier { get; set; }
        public string AreaName { get; set; }
        public string AreaDisplayTitle { get; set; }
        public string Prologue { get; set; }
        public string Epilogue { get; set; }
        public string EmailTemplate { get; set; }
        public bool IsEnabled { get; set; }
        public List<StaticTablesMeta> StaticTablesMetas { get; set; } = new List<StaticTablesMeta>();
        public List<ConversationNode> ConversationNodes { get; set; } = new List<ConversationNode>();
        public string AccountId { get; set; }
        public List<DynamicTableMeta> DynamicTableMetas { get; set; } = new List<DynamicTableMeta>();
        public string AreaSpecificEmail { get; set; }
        public bool EmailIsVerified { get; set; }
        public List<AttachmentLinkRecord> AttachmentRecords { get; set; }

        public bool UseAreaFallbackEmail { get; set; }
        public string FallbackSubject { get; set; } = null!;
        public string FallbackEmailTemplate { get; set; } = null!;
        public bool SendAttachmentsOnFallback { get; set; }
        public bool SendPdfResponse { get; set; } = true;
        public bool IncludeDynamicTableTotals { get; set; }

        public string Subject { get; set; } = null!;
    }
}