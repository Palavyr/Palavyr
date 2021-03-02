﻿using System.Collections.Generic;
using Palavyr.API.Controllers.Accounts.Setup.SeedData.DataCreators;
using Palavyr.Common.UIDUtils;
using Palavyr.Data.Setup.SeedData.DataCreators;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Data.Setup.SeedData
{
    public abstract class BaseSeedData
    {
        public List<Area> Areas { get; set; }
        public WidgetPreference WidgetPreference { get; set; }
        public List<ConversationNode> DefaultConversationNodes { get; set; }
        public List<DynamicTableMeta> DefaultDynamicTableMetas { get; set; } = new List<DynamicTableMeta>();
        public readonly List<SelectOneFlat> DefaultDynamicTables = new List<SelectOneFlat>();

        public const string AreaName = "Buying a Cavvy";
        const string TableTag = "Cavvy Types";
        public string EmailTemplate => CreateEmailTemplate.Create();
        
        
        protected BaseSeedData(string accountId, string defaultEmail)
        {
            var areaIdentifier = GuidUtils.CreateNewId();
            var dynamicTableId = GuidUtils.CreateNewId();

            DefaultConversationNodes =
                CreateDefaultConversation.CreateDefault(accountId, areaIdentifier, dynamicTableId);
            DefaultDynamicTables =
                CreateDefaultDynamicTable.CreateDefaultTable(TableTag, accountId, areaIdentifier, dynamicTableId);
            DefaultDynamicTableMetas =
                CreateDefaultDynamicTable.CreateDefaultMeta(TableTag, accountId, dynamicTableId, areaIdentifier);
            WidgetPreference = WidgetPreference.CreateDefault(accountId);
            Areas = new List<Area>()
            {
                CreateDefaultArea.CreateDefault(
                    areaIdentifier,
                    accountId,
                    AreaName,
                    DefaultConversationNodes,
                    DefaultDynamicTableMetas,
                    EmailTemplate,
                    defaultEmail
                    )
            };
        }
    }
}