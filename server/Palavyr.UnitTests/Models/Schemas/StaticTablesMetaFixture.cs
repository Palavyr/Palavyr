using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;
using Shouldly;
using Xunit;

namespace PalavyrServer.UnitTests.Models.Schemas
{
    [Trait("Static Tables", "Meta")]
    public class StaticTablesMetaFixture
    {
        [Fact]
        public void BindTemplateList_BindsAllProperties()
        {
            var testAccountId = "test-account";
            var testAreaId = "test-areaId";

            var testStaticTablesMetas = StaticTablesMeta.CreateDefaultMetas(testAreaId, testAccountId);
            
            var result = StaticTablesMeta.BindTemplateList(testStaticTablesMetas, testAccountId).First();
            
            result.AccountId.ShouldBe(testAccountId);
            result.AreaIdentifier.ShouldBe(testAreaId);
            result.Description.ShouldBe("Default Description");
            result.TableOrder.ShouldBe(0);
        }
    }
}