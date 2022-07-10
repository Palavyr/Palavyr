﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.API.Controllers.Response.Tables.Dynamic;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.DataCreators;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Validators
{
    public class WhenCreatingASelectOneFlatTable : RealDatabaseIntegrationFixture
    {
        [Fact]
        public async Task ARoundTripShouldSucceed()
        {
            var selectOneFlatTableMeta = await this.CreatePricingStrategyTableBuilder<SelectOneFlatResource>()
                .WithRow(this.CreateSelectOneFlatResourceBuilder().Build())
                .BuildAndMake<SelectOneFlat, SelectOneFlatCompiler>();

            var getRoute = PricingStrategyControllerBase<SelectOneFlat,
                SelectOneFlatResource,
                SelectOneFlatCompiler>.AssembleRoute<SelectOneFlat>(
                GetPricingStrategyTableRowsRequest<
                        SelectOneFlat,
                        SelectOneFlatResource,
                        SelectOneFlatCompiler>
                    .FormatRoute(selectOneFlatTableMeta.AreaIdentifier, selectOneFlatTableMeta.TableId));

            var currentTable = await Client.GetResource<GetPricingStrategyTableRowsRequest<
                SelectOneFlat,
                SelectOneFlatResource,
                SelectOneFlatCompiler>, PricingStrategyTableDataResource<SelectOneFlatResource>>(CancellationToken, _ => getRoute);

            currentTable.TableRows.First().AccountId.ShouldBe(this.AccountId);
        }

        public WhenCreatingASelectOneFlatTable(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}