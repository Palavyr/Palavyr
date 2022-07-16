using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using IntegrationTests.DataCreators;
using Palavyr.API.Controllers.Response.Tables.Dynamic;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests.Core.Validators
{
    public class WhenCreatingASelectOneFlatTable : IntegrationTest
    {
        [Fact]
        public async Task ARoundTripShouldSucceed()
        {
            var selectOneFlatTableMeta = await this.CreatePricingStrategyTableBuilder<SelectOneFlatResource>()
                .WithRow(this.CreateSelectOneFlatResourceBuilder().Build())
                .BuildAndMake<SimpleSelectTableRow, SelectOneFlatCompiler>();

            var getRoute = PricingStrategyControllerBase<SimpleSelectTableRow,
                SelectOneFlatResource,
                SelectOneFlatCompiler>.AssembleRoute<SimpleSelectTableRow>(
                GetPricingStrategyTableRowsRequest<
                        SimpleSelectTableRow,
                        SelectOneFlatResource,
                        SelectOneFlatCompiler>
                    .FormatRoute(selectOneFlatTableMeta.AreaIdentifier, selectOneFlatTableMeta.TableId));

            var currentTable = await Client.GetResource<GetPricingStrategyTableRowsRequest<
                SimpleSelectTableRow,
                SelectOneFlatResource,
                SelectOneFlatCompiler>, PricingStrategyTableDataResource<SelectOneFlatResource>>(CancellationToken, _ => getRoute);

            currentTable.TableRows.First().AccountId.ShouldBe(this.AccountId);
        }

        public WhenCreatingASelectOneFlatTable(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}