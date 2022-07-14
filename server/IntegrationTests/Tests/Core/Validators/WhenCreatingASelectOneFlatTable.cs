using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using IntegrationTests.DataCreators;
using Palavyr.API.Controllers.Response.Tables.Dynamic;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
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