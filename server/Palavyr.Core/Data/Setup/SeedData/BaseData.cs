using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Data.Setup.SeedData.DataCreators;

namespace Palavyr.Core.Data.Setup.SeedData
{
    public abstract class BaseSeedData
    {
        public List<Intent> Intents { get; set; }
        public WidgetPreference WidgetPreference { get; set; }
        public List<ConversationNode> DefaultConversationNodes { get; set; }
        public List<ConversationNode> IntroductionConversationNodes { get; set; }

        public List<PricingStrategyTableMeta> DefaultPricingStrategyTableMetas { get; set; }
        public readonly List<CategorySelectTableRow> DefaultPricingStrategyTables;

        public const string IntentName = "Buying a Dog";
        private const string TableTag = "Dog Color Types";
        public string EmailTemplate => CreateEmailTemplate.Create();


        protected BaseSeedData(string accountId, string defaultEmail, string introId)
        {
            var intentId = StaticGuidUtils.CreateNewId();
            var pricingStrategyTableId = StaticGuidUtils.CreateNewId();

            DefaultConversationNodes = CreateDefaultConversation.CreateDefault(accountId, intentId, pricingStrategyTableId);

            IntroductionConversationNodes = ConversationNode.CreateDefaultRootNode(introId, accountId);

            DefaultPricingStrategyTables = CreateDefaultPricingStrategyTable.CreateDefaultTable(TableTag, accountId, intentId, pricingStrategyTableId);
            DefaultPricingStrategyTableMetas = CreateDefaultPricingStrategyTable.CreateDefaultMeta(TableTag, accountId, pricingStrategyTableId, intentId);
            WidgetPreference = WidgetPreference.CreateDefault(accountId);
            Intents = new List<Intent>
            {
                CreateDefaultIntent.CreateDefault(
                    intentId,
                    accountId,
                    IntentName,
                    DefaultConversationNodes,
                    DefaultPricingStrategyTableMetas,
                    EmailTemplate,
                    defaultEmail
                )
            };
        }
    }
}