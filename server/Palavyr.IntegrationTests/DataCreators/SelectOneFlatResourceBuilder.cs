using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static partial class BuilderExtensionMethods
    {
        public static SelectOneFlatResourceBuilder CreateSelectOneFlatResourceBuilder(this BaseIntegrationFixture test)
        {
            return new SelectOneFlatResourceBuilder();
        }
    }
    
    public class SelectOneFlatResourceBuilder
    {
        private string? accountId;
        private string? intentId;
        private string? tableId;
        private string? option;
        private double? valueMin;
        private double? valueMax;
        private bool? range;
        private int? rowOrder;

        public SelectOneFlatResourceBuilder()
        {
        }

        public SelectOneFlatResourceBuilder WithAccountId(string accountId)
        {
            this.accountId = accountId;
            return this;
        }

        public SelectOneFlatResourceBuilder WithIntentId(string intentId)
        {
            this.intentId = intentId;
            return this;
        }

        public SelectOneFlatResourceBuilder WithTableId(string tableId)
        {
            this.tableId = tableId;
            return this;
        }

        public SelectOneFlatResourceBuilder WithOption(string option)
        {
            this.option = option;
            return this;
        }

        public SelectOneFlatResourceBuilder WithValueMin(double valueMin)
        {
            this.valueMin = valueMin;
            return this;
        }

        public SelectOneFlatResourceBuilder WithValueMax(double valueMax)
        {
            this.valueMax = valueMax;
            return this;
        }

        public SelectOneFlatResourceBuilder WithRange(bool range)
        {
            this.range = range;
            return this;
        }

        public SelectOneFlatResourceBuilder WithRowOrder(int rowOrder)
        {
            this.rowOrder = rowOrder;
            return this;
        }

        public SelectOneFlatResource Build()
        {
            var resource = new SelectOneFlatResource()
            {
                AccountId = this.accountId ?? A.RandomId(),
                AreaIdentifier = this.intentId ?? A.RandomId(),
                TableId = this.tableId ?? A.RandomId(),
                Option = this.option ?? A.RandomString(),
                ValueMin = this.valueMin ?? A.RandomInt(0, 10),
                ValueMax = this.valueMax ?? A.RandomInt(11, 20), // no overlap with min
                Range = this.range ?? false,
                RowOrder = this.rowOrder ?? 0,
            };
            return resource;
        }
    }
}