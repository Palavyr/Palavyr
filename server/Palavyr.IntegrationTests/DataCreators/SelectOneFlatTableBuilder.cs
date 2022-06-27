using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static partial class BuilderExtensionMethods
    {
        public static SelectOneFlatTableBuilder CreateSelectOneFlatTableBuilder(this BaseIntegrationFixture test)
        {
            return new SelectOneFlatTableBuilder(test);
        }
    }

    public class SelectOneFlatTableBuilder
    {
        private readonly BaseIntegrationFixture test;
        private readonly List<SelectOneFlatResource> table = new List<SelectOneFlatResource>();
        private string? intentId;

        public SelectOneFlatTableBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public SelectOneFlatTableBuilder WithIntent(string intentId)
        {
            this.intentId = intentId;
            return this;
        }

        public SelectOneFlatTableBuilder WithRow(SelectOneFlatResource rowResource)
        {
            table.Add(rowResource);
            return this;
        }

        public SelectOneFlatTableBuilder WithRow(Func<SelectOneFlatResource> func)
        {
            var row = func.Invoke();
            table.Add(row);
            return this;
        }

        public SelectOneFlatTableBuilder WithRows(IEnumerable<SelectOneFlatResource> rows)
        {
            table.AddRange(rows);
            return this;
        }

        public List<SelectOneFlatResource> Build()
        {
            return table;
        }

        public async Task<List<SelectOneFlatResource>> BuildAndMake()
        {
            var response = await test
                .Client
                .Post<CreatePricingStrategyTableRequest<
                        SelectOneFlat,
                        SelectOneFlatResource,
                        SelectOneFlatCompiler>,
                    List<SelectOneFlatResource>>(
                    table,
                    test.CancellationToken,
                    route => route.Replace("{intentId}", intentId ?? A.RandomId()));
            return response;
        }
    }
}