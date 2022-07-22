using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Services.StripeServices.Products;
using Shouldly;
using Xunit;

namespace Palavyr.PureTests.Core.Services.StripeServices
{
    public class ProductionProductRegistryFixture : IAsyncLifetime
    {
        private ProductionProductRegistry registry;

        [Fact]
        public void WhenGettingProductIds_AllProductTypesAreSet()
        {
            var productIds = registry.GetProductIds();

            productIds.FreeProductId.ShouldBe("");
            productIds.LyteProductId.ShouldBe("prod_JbRdiPSPXeEBjh");
            productIds.PremiumProductId.ShouldBe("prod_JbRfeWiHYbkoks");
            productIds.ProProductId.ShouldBe("prod_JbRySZg5wrSNvX");
        }

        [Fact]
        public void WhenGivenTheFreeProductId_TheFreePlanTypeEnumIsReturned()
        {
            var planTypeEnum = registry.GetPlanTypeEnum(ProductionProducts.FreeProduct.FreeProductId);
            planTypeEnum.ShouldBe(Account.PlanTypeEnum.Free);
        }

        [Fact]
        public void WhenGivenTheLyteProductId_TheLytePlanTypeEnumIsReturned()
        {
            var planTypeEnum = registry.GetPlanTypeEnum(ProductionProducts.LyteProduct.LyteProductId);
            planTypeEnum.ShouldBe(Account.PlanTypeEnum.Lyte);
        }

        [Fact]
        public void WhenGivenThePremiumProductId_ThePremiumPlanTypeEnumIsReturned()
        {
            var planTypeEnum = registry.GetPlanTypeEnum(ProductionProducts.PremiumProduct.PremiumProductId);
            planTypeEnum.ShouldBe(Account.PlanTypeEnum.Premium);
        }

        [Fact]
        public void WhenGettingTheProPlanEnumType_TheProPlanTypeEnumIsReturned()
        {
            var planTypeEnum = registry.GetPlanTypeEnum(ProductionProducts.ProProduct.ProProductId);
            planTypeEnum.ShouldBe(Account.PlanTypeEnum.Pro);
        }

        public async Task InitializeAsync()
        {
            registry = new ProductionProductRegistry();
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}