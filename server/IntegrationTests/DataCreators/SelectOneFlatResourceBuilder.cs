using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;
using Test.Common.Random;

namespace IntegrationTests.DataCreators
{
    public static partial class BuilderExtensionMethods
    {
        public static SelectOneFlatResourceBuilder CreateSelectOneFlatResourceBuilder(this IntegrationTest test)
        {
            return new SelectOneFlatResourceBuilder(test);
        }
    }

    public class SelectOneFlatResourceBuilder
    {
        private readonly IntegrationTest test;
        private string? accountId;
        private string? intentId;
        private string? tableId;
        private string? option;
        private double? valueMin;
        private double? valueMax;
        private bool? range;
        private int? rowOrder;

        public SelectOneFlatResourceBuilder(IntegrationTest test)
        {
            this.test = test;
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

        public CategorySelectTableRowResource Build()
        {
            var intentid = this.intentId;

            var resource = new CategorySelectTableRowResource()
            {
                AccountId = this.accountId ?? test.AccountId,
                IntentId = intentid ?? A.RandomId(),
                TableId = this.tableId ?? A.RandomId(),
                Category = this.option ?? A.RandomString(),
                ValueMin = this.valueMin ?? A.RandomInt(0, 10),
                ValueMax = this.valueMax ?? A.RandomInt(11, 20), // no overlap with min
                Range = this.range ?? false,
                RowOrder = this.rowOrder ?? 0,
            };
            return resource;
        }

        public async Task<List<CategorySelectTableRowResource>> BuildAndCreate()
        {
            var resource = Build();

            var newTable = await test
                .Client
                .Post<CreatePricingStrategyTableRequest<CategorySelectTableRow, CategorySelectTableRowResource, SelectOneFlatCompiler>, List<CategorySelectTableRowResource>>(
                    test.CancellationToken,
                    s => CreatePricingStrategyTableRequest<CategorySelectTableRow, CategorySelectTableRowResource, SelectOneFlatCompiler>.FormatRoute(resource.IntentId));


            newTable.Add(resource);
            var response = await test.Client
                .Post<SavePricingStrategyTableRequest<CategorySelectTableRow, CategorySelectTableRowResource, SelectOneFlatCompiler>, SavePricingStrategyTableResponse<CategorySelectTableRowResource>>(
                    newTable,
                    test.CancellationToken,
                    s => SavePricingStrategyTableRequest<CategorySelectTableRow, CategorySelectTableRowResource, SelectOneFlatCompiler>.FormatRoute(intentId ?? newTable.First().IntentId, newTable.First().TableId));
            newTable = response.Resource.TableRows;
            return newTable;
        }
    }
}