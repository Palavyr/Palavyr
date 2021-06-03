using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.StripeServices.Products;
using Shouldly;
using Xunit;

namespace PalavyrServer.UnitTests.Core.Services.StripeServices
{
    public class StagingProductRegistryFixture
    {
        [Fact]
        public void WhenGettingProductIds_AllProductTypesAreSet()
        {
            var registry = new StagingProductRegistry();
            var productIds = registry.GetProductIds();

            productIds.FreeProductId.ShouldBe("");
            productIds.LyteProductId.ShouldBe("prod_JZUQhoJgFpDOxk");
            productIds.PremiumProductId.ShouldBe("prod_JZUR7wpyLfcovm");
            productIds.ProProductId.ShouldBe("prod_JZUSwAsLiCwjus");
        }

        [Fact]
        public void WhenGivenTheFreeProductId_TheFreePlanTypeEnumIsReturned()
        {
            var registry = new StagingProductRegistry();

            var planTypeEnum = registry.GetPlanTypeEnum(StagingProducts.FreeProduct.FreeProductId);
            planTypeEnum.ShouldBe(Account.PlanTypeEnum.Free);
        }

        [Fact]
        public void WhenGivenTheLyteProductId_TheLytePlanTypeEnumIsReturned()
        {
            var registry = new StagingProductRegistry();

            var planTypeEnum = registry.GetPlanTypeEnum(StagingProducts.LyteProduct.LyteProductId);
            planTypeEnum.ShouldBe(Account.PlanTypeEnum.Lyte);
        }

        [Fact]
        public void WhenGivenThePremiumProductId_ThePremiumPlanTypeEnumIsReturned()
        {
            var registry = new StagingProductRegistry();

            var planTypeEnum = registry.GetPlanTypeEnum(StagingProducts.PremiumProduct.PremiumProductId);
            planTypeEnum.ShouldBe(Account.PlanTypeEnum.Premium);
        }


        [Fact]
        public void WhenGettingTheProPlanEnumType_TheProPlanTypeEnumIsReturned()
        {
            var registry = new StagingProductRegistry();

            var planTypeEnum = registry.GetPlanTypeEnum(StagingProducts.ProProduct.ProProductId);
            planTypeEnum.ShouldBe(Account.PlanTypeEnum.Pro);
        }
    }
}