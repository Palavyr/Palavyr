﻿using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Setup.SeedData.DataCreators;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.Core.Data.Setup.SeedData
{
    public abstract class BaseSeedData
    {
        public List<Area> Areas { get; set; }
        public WidgetPreference WidgetPreference { get; set; }
        public List<ConversationNode> DefaultConversationNodes { get; set; }
        public List<ConversationNode> IntroductionConversationNodes { get; set; }
        
        public List<DynamicTableMeta> DefaultDynamicTableMetas { get; set; } = new List<DynamicTableMeta>();
        public readonly List<SelectOneFlat> DefaultDynamicTables = new List<SelectOneFlat>();
        

        public const string AreaName = "Buying a Dpg";
        private const string TableTag = "Dog Color Types";
        public string EmailTemplate => CreateEmailTemplate.Create();


        protected BaseSeedData(string accountId, string defaultEmail, string introId)
        {
            var areaIdentifier = StaticGuidUtils.CreateNewId();
            var dynamicTableId = StaticGuidUtils.CreateNewId();

            DefaultConversationNodes = CreateDefaultConversation.CreateDefault(accountId, areaIdentifier, dynamicTableId);

            IntroductionConversationNodes = ConversationNode.CreateDefaultRootNode(introId, accountId);
                
                
            DefaultDynamicTables = CreateDefaultDynamicTable.CreateDefaultTable(TableTag, accountId, areaIdentifier, dynamicTableId);
            DefaultDynamicTableMetas = CreateDefaultDynamicTable.CreateDefaultMeta(TableTag, accountId, dynamicTableId, areaIdentifier);
            WidgetPreference = WidgetPreference.CreateDefault(accountId);
            Areas = new List<Area>
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