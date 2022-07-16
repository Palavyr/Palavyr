﻿#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public List<StaticTablesMeta> StaticTablesMetas { get; set; } = new List<StaticTablesMeta>();
        public List<ConversationNode> ConversationNodes { get; set; } = new List<ConversationNode>();
        public string AccountId { get; set; }
        public List<PricingStrategyTableMeta> DynamicTableMetas { get; set; } = new List<PricingStrategyTableMeta>();
        public string IntentSpecificEmail { get; set; }
        public bool EmailIsVerified { get; set; }
        public List<AttachmentLinkRecord> AttachmentRecords { get; set; }

        public bool UseIntentFallbackEmail { get; set; }
        public string FallbackSubject { get; set; } = null!;
        public string FallbackEmailTemplate { get; set; } = null!;
        public bool SendAttachmentsOnFallback { get; set; }
        public bool SendPdfResponse { get; set; } = true;
        public bool IncludeDynamicTableTotals { get; set; }

        [DefaultValue("Thank you for reaching out!")]
        public string Subject { get; set; } = null!;

        [NotMapped]
        public bool AwaitingVerification { get; set; }

        public Intent()
        {
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
                DynamicTableMetas = null,
                IntentSpecificEmail = emailAddress,
                EmailIsVerified = emailIsVerified,
                SendPdfResponse = true,
                IncludeDynamicTableTotals = true
            };
        }
    }
}