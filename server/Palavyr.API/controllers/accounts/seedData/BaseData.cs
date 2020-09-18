using System;
using System.Collections.Generic;
using Palavyr.API.controllers.accounts.seedData.dataCreators;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    public abstract class BaseSeedData
    {
        public List<Area> Areas { get; set; }
        public List<GroupMap> Groups { get; set; } = new List<GroupMap>();
        public WidgetPreference WidgetPreference { get; set; }
        public List<ConversationNode> DefaultConversationNodes { get; set; }
        public List<DynamicTableMeta> DefaultDynamicTableMetas { get; set; } = new List<DynamicTableMeta>();
        public readonly List<SelectOneFlat> DefaultDynamicTables = new List<SelectOneFlat>();

        public const string AreaName = "Buying a Cavvy";
        const string TableTag = "Cavvy Types";
        public string EmailTemplate => CreateEmailTemplate.Create();
        
        
        protected BaseSeedData(string accountId)
        {
            var areaIdentifier = Guid.NewGuid().ToString();
            var GroupId = Guid.NewGuid().ToString();
            var dynamicTableId = Guid.NewGuid().ToString();

            DefaultConversationNodes =
                CreateDefaultConversation.CreateDefault(accountId, areaIdentifier, dynamicTableId);
            DefaultDynamicTables =
                CreateDefaultDynamicTable.CreateDefaultTable(TableTag, accountId, areaIdentifier, dynamicTableId);
            DefaultDynamicTableMetas =
                CreateDefaultDynamicTable.CreateDefaultMeta(TableTag, accountId, dynamicTableId, areaIdentifier);
            WidgetPreference = WidgetPreference.CreateNew("Tobies Galore", "Experts in Cavalier King Charles Spaniels",
                "Write here...", false, accountId);
            Groups.Add(GroupMap.CreateGroupMap(GroupId, null, "Group One", accountId));
            Areas = new List<Area>()
            {
                CreateDefaultArea.CreateDefault(
                    areaIdentifier,
                    accountId,
                    AreaName,
                    DefaultConversationNodes,
                    DefaultDynamicTableMetas,
                    GroupId,
                    EmailTemplate)
            };
        }
    }
}