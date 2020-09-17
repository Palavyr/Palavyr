using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Server.Domain;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    public class SeedData
    {
        public List<Area> Areas { get; set; }
        public List<GroupMap> Groups { get; set; } = new List<GroupMap>();
        public WidgetPreference WidgetPreference { get; set; }
        public List<ConversationNode> DefaultConversationNodes { get; set; }
        public List<DynamicTableMeta> DefaultDynamicTableMetas { get; set; } = new List<DynamicTableMeta>();
        public readonly List<SelectOneFlat> DefaultDynamicTables = new List<SelectOneFlat>();
        
        public SeedData(string accountId)
        {
            var areaIdentifier = Guid.NewGuid().ToString();
            const string areaName = "Buying a Cavvy";
            const string dynamicTableId = "h1h3-5k32k-5h3k-35l3";
            const string tableTag = "Cavvy Types";
            const string emailTemplate = "<div> Howdy! </div>";
            var groupId = Guid.NewGuid().ToString();

            DefaultConversationNodes = CreateDefaultConversation.CreateDefault(accountId, areaIdentifier, dynamicTableId);
            DefaultDynamicTables = CreateDefaultDynamicTable.CreateDefaultTable(tableTag, accountId, areaIdentifier, dynamicTableId);
            DefaultDynamicTableMetas = CreateDefaultDynamicTable.CreateDefaultMeta(tableTag, accountId, dynamicTableId, areaIdentifier);
            WidgetPreference = WidgetPreference.CreateNew("Tobies Galore", "Experts in Cavalier King Charles Spaniels", "Write here...", false, accountId);
            Groups.Add(GroupMap.CreateGroupMap(groupId, null, "Group One", accountId));
            Areas = new List<Area>()
            {
                CreateDefaultArea.CreateDefault(
                    areaIdentifier,
                    accountId,
                    areaName,
                    DefaultConversationNodes,
                    DefaultDynamicTableMetas,
                    groupId,
                    emailTemplate)
            };
        }
    }
}