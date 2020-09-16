using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Server.Domain.App.schema;

namespace Server.Domain
{
    public class Area
    {
        [Key] 
        public int Id { get; set; }
        public string AreaIdentifier { get; set; }
        public string AreaName { get; set; }
        public string AreaDisplayTitle { get; set; }
        public string Prologue { get; set; }
        public string Epilogue { get; set; }
        public string EmailTemplate { get; set; }
        public bool IsComplete { get; set; }
        public List<StaticTablesMeta> StaticTablesMetas { get; set; } = new List<StaticTablesMeta>();
        public List<ConversationNode> ConversationNodes { get; set; } = new List<ConversationNode>();
        public string GroupId { get; set; }
        public string AccountId { get; set; }
        public List<DynamicTableMeta> DynamicTableMetas { get; set; } = new List<DynamicTableMeta>();
        
        public static Area CreateNewArea(string areaName, string accountId)
        {
            var areaId = Guid.NewGuid().ToString();
            return new Area()
            {
                AreaIdentifier = areaId,
                AreaName = areaName,
                Prologue = "Initial Prologue",
                Epilogue = "Initial Epilogue",
                EmailTemplate = "<h2>Upload your custom email template</h2>",
                ConversationNodes = ConversationNode.CreateDefaultNode(areaId, accountId),
                StaticTablesMetas = StaticTablesMeta.CreateDefaultMetas(areaId, accountId),
                IsComplete = false,
                AreaDisplayTitle = "A short display title used with the widget.",
                AccountId = accountId,
                DynamicTableMetas = DynamicTableMeta.CreateDefaultMetas(areaId, accountId)
            };
        }
    }
}