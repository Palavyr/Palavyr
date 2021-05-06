﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class Area
    {
        [Key] public int? Id { get; set; }
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

        public bool UseAreaFallbackEmail { get; set; }
        public string FallbackSubject { get; set; } = null!;
        public string FallbackEmailTemplate { get; set; } = null!;
        public bool SendAttachmentsOnFallback { get; set; }
        
        
        [DefaultValue("Thank you for reaching out!")]
        public string Subject { get; set; } = null!;

        [NotMapped] public bool AwaitingVerification { get; set; }

        public Area()
        {
        }
        public static Area CreateNewArea(string areaName, string accountId, string emailAddress, bool emailIsVerified)
        {
            var areaId = Guid.NewGuid().ToString();
            return new Area()
            {
                AreaIdentifier = areaId,
                AreaName = areaName,
                Prologue = "",
                Epilogue = "",
                EmailTemplate = "<h2>Upload your custom email template</h2><p>This is a starter paragraph to get you started</p>",
                Subject = "Thank you for reaching out!",
                FallbackEmailTemplate = "<h2>Fallback email</h2>",
                FallbackSubject = "Fallback Subject",
                ConversationNodes = ConversationNode.CreateDefaultNode(areaId, accountId),
                StaticTablesMetas = new List<StaticTablesMeta>(),
                IsEnabled = false,
                AreaDisplayTitle = areaName,
                AccountId = accountId,
                DynamicTableMetas = null,
                AreaSpecificEmail = emailAddress,
                EmailIsVerified = emailIsVerified
            };
        }
    }
}