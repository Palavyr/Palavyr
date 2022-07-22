

using System;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class StaticFee : Entity, IRecord, IHaveAccountId
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public string FeeId { get; set; }
        public string AccountId { get; set; }
        public string IntentId { get; set; }

        public static StaticFee DefaultFee(string accountId, string intentId)
        {
            return new StaticFee()
            {
                Min = 0,
                Max = 0,
                FeeId = Guid.NewGuid().ToString(),
                AccountId = accountId,
                IntentId = intentId
            };
        }

        public static StaticFee BindTemplate(StaticFee oldFee, string accountId, string intentId)
        {
            return new StaticFee()
            {
                Min = oldFee.Min,
                Max = oldFee.Max,
                FeeId = oldFee.FeeId,
                AccountId = accountId,
                IntentId = intentId
            };
        }

        public static StaticFee CreateNew(double min, double max, string feeId, string accountId, string intentId)
        {
            return new StaticFee()
            {
                Min = min,
                Max = max,
                FeeId = feeId,
                AccountId = accountId,
                IntentId = intentId
            };
        }
    }
}