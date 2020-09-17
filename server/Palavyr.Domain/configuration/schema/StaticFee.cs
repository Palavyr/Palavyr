using System;

namespace Server.Domain.Configuration.schema
{
    public class StaticFee
    {
        public int Id { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public string FeeId { get; set; }
        public string AccountId { get; set; }

        public static StaticFee DefaultFee(string accountId)
        {
            return new StaticFee()
            {
                Min = 0,
                Max = 0,
                FeeId = Guid.NewGuid().ToString(),
                AccountId = accountId
            };
        }

        public static StaticFee BindTemplate(StaticFee oldFee, string accountId)
        {
            return new StaticFee()
            {
                Min = oldFee.Min,
                Max = oldFee.Max,
                FeeId = oldFee.FeeId,
                AccountId = accountId
            };
        }
    }
}