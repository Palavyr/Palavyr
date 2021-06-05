using Palavyr.Core.Models.Accounts.Schemas;
using Shouldly;
using Xunit;

namespace PalavyrServer.UnitTests.Core.Models.Accounts.Schemas
{
    public class AccountFixture
    {
        [Fact]
        public void PlanTypeEnumIsCorrect()
        {
            ((int)Account.PlanTypeEnum.Free).ShouldBe(0);
            ((int)Account.PlanTypeEnum.Lyte).ShouldBe(1);
            ((int)Account.PlanTypeEnum.Premium).ShouldBe(2);
            ((int)Account.PlanTypeEnum.Pro).ShouldBe(3);
        }

        [Fact]
        public void PlanTypeIsCorrect()
        {
            Account.PlanTypes.Lyte.ShouldBe("Lyte");
            Account.PlanTypes.Premium.ShouldBe("Premium");
            Account.PlanTypes.Pro.ShouldBe("Pro");
        }

        [Fact]
        public void PaymentIntervalsAreCorrect()
        {
            Account.PaymentIntervals.Month.ShouldBe("month");
            Account.PaymentIntervals.Year.ShouldBe("year");
        }

        [Fact]
        public void PaymentIntervalsEnumIsCorrect()
        {
            ((int)Account.PaymentIntervalEnum.Null).ShouldBe(0);
            ((int)Account.PaymentIntervalEnum.Month).ShouldBe(1);
            ((int)Account.PaymentIntervalEnum.Year).ShouldBe(2);
        }
    }
}