using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;

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

        public SelectOneFlatTableBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
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
            await test.Client.PostAsync()
        }
    }
}