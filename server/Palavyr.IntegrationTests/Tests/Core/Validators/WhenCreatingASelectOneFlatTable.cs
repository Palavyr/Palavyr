using System.Linq;
using System.Threading.Tasks;
using Palavyr.API.Controllers.Response.Tables.PricingStrategy;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
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
    public class WhenCreatingASelectOneFlatTable : IntegrationTest
    {
        [Fact]
        public async Task ARoundTripShouldSucceed()
        {
            var selectOneFlatTableMeta = await this.CreatePricingStrategyTableBuilder<CategorySelectTableRowResource>()
                .WithRow(this.CreateSelectOneFlatResourceBuilder().Build())
                .BuildAndMake<CategorySelectTableRow, SelectOneFlatCompiler>();

            var getRoute = PricingStrategyControllerBase<CategorySelectTableRow,
                CategorySelectTableRowResource,
                SelectOneFlatCompiler>.AssembleRoute<CategorySelectTableRow>(
                GetPricingStrategyTableRowsRequest<
                        CategorySelectTableRow,
                        CategorySelectTableRowResource,
                        SelectOneFlatCompiler>
                    .FormatRoute(selectOneFlatTableMeta.IntentId, selectOneFlatTableMeta.TableId));

            var currentTable = await Client.GetResource<GetPricingStrategyTableRowsRequest<
                CategorySelectTableRow,
                CategorySelectTableRowResource,
                SelectOneFlatCompiler>, PricingStrategyTableDataResource<CategorySelectTableRowResource>>(CancellationToken, _ => getRoute);

            currentTable.TableRows.First().TableId.ShouldBe(selectOneFlatTableMeta.TableId);
        }

        public WhenCreatingASelectOneFlatTable(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}