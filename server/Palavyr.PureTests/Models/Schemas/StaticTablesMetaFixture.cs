using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Data.Entities;
using Shouldly;
using Xunit;

namespace Palavyr.PureTests.Models.Schemas
{
    [Trait("Static Tables", "Meta")]
    public class StaticTablesMetaFixture
    {
        [Fact]
        public void BindTemplateList_BindsAllProperties()
        {
            var testAccountId = "test-account";
            var testIntentId = "test-intentId";

            var testStaticTablesMetas = new List<StaticTablesMeta>()
            {
                StaticTablesMeta.CreateNewMetaTemplate(testIntentId, testAccountId)
            };
            
            var result = StaticTablesMeta.BindTemplateList(testStaticTablesMetas, testAccountId).First();
            
            result.AccountId.ShouldBe(testAccountId);
            result.IntentId.ShouldBe(testIntentId);
            result.Description.ShouldBe("Default Description");
            result.TableOrder.ShouldBe(0);
        }
    }
}