using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Data.Setup.SeedData.DataCreators;

namespace Palavyr.Core.Data.Setup.SeedData
{
    public abstract class BaseSeedData
    {
        public List<Intent> Areas { get; set; }
        public WidgetPreference WidgetPreference { get; set; }
        public List<ConversationNode> DefaultConversationNodes { get; set; }
        public List<ConversationNode> IntroductionConversationNodes { get; set; }

        public List<PricingStrategyTableMeta> DefaultDynamicTableMetas { get; set; } = new List<PricingStrategyTableMeta>();
        public readonly List<SimpleSelectTableRow> DefaultDynamicTables = new List<SimpleSelectTableRow>();


        public const string AreaName = "Buying a Dog";
        private const string TableTag = "Dog Color Types";
        public string EmailTemplate => CreateEmailTemplate.Create();


        protected BaseSeedData(string accountId, string defaultEmail, string introId)
        {
            var intentId = StaticGuidUtils.CreateNewId();
            var pricingStrategyTableId = StaticGuidUtils.CreateNewId();

            DefaultConversationNodes = CreateDefaultConversation.CreateDefault(accountId, intentId, pricingStrategyTableId);

            IntroductionConversationNodes = ConversationNode.CreateDefaultRootNode(introId, accountId);

            DefaultDynamicTables = CreateDefaultDynamicTable.CreateDefaultTable(TableTag, accountId, intentId, pricingStrategyTableId);
            DefaultDynamicTableMetas = CreateDefaultDynamicTable.CreateDefaultMeta(TableTag, accountId, pricingStrategyTableId, intentId);
            WidgetPreference = WidgetPreference.CreateDefault(accountId);
            Areas = new List<Intent>
            {
                CreateDefaultArea.CreateDefault(
                    intentId,
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