using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class Intent : Entity, IHaveAccountId
    {
        public string IntentId { get; set; }
        public string IntentName { get; set; }
        public string Prologue { get; set; }
        public string Epilogue { get; set; }
        public string EmailTemplate { get; set; }
        public bool IsEnabled { get; set; }
        public List<StaticTablesMeta> StaticTablesMetas { get; set; } = new();
        public List<ConversationNode> ConversationNodes { get; set; } = new();
        public string AccountId { get; set; }
        public List<PricingStrategyTableMeta> PricingStrategyTableMetas { get; set; } = new();
        public string IntentSpecificEmail { get; set; }
        public bool EmailIsVerified { get; set; }
        public List<AttachmentLinkRecord> AttachmentRecords { get; set; } = new();
        public bool UseIntentFallbackEmail { get; set; }
        public string FallbackSubject { get; set; }
        public string FallbackEmailTemplate { get; set; }
        public bool SendAttachmentsOnFallback { get; set; }
        public bool SendPdfResponse { get; set; } = true;
        public bool IncludePricingStrategyTableTotals { get; set; }
        public string Subject { get; set; }

        [NotMapped]
        public bool AwaitingVerification { get; set; }

        public Intent()
        {
        }

        public Intent UpdateIntentId(string intentId)
        {
            IntentId = intentId;
            return this;
        }
        
        public static Intent CreateNewIntent(string intentName, string accountId, string emailAddress, bool emailIsVerified)
        {
            var intentId = Guid.NewGuid().ToString();
            return new Intent
            {
                IntentId = intentId,
                IntentName = intentName,
                Prologue = "",
                Epilogue = "",
                EmailTemplate = "<h2>Upload your custom email template</h2><p>This is a starter paragraph to get you started</p>",
                Subject = "Thank you for reaching out!",
                FallbackEmailTemplate = "<h2>Fallback email</h2>",
                FallbackSubject = "Fallback Subject",
                ConversationNodes = ConversationNode.CreateDefaultRootNode(intentId, accountId),
                StaticTablesMetas = new List<StaticTablesMeta>(),
                IsEnabled = false,
                AccountId = accountId,
                PricingStrategyTableMetas = new List<PricingStrategyTableMeta>(),
                IntentSpecificEmail = emailAddress,
                EmailIsVerified = emailIsVerified,
                SendPdfResponse = true,
                IncludePricingStrategyTableTotals = true,
                AttachmentRecords = new List<AttachmentLinkRecord>(),
                UseIntentFallbackEmail = false,
                SendAttachmentsOnFallback = false
            };
        }
    }
}