using System;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class StaticFee : Entity, IRecord
    {
       
        public double Min { get; set; }
        public double Max { get; set; }
        public string FeeId { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }

        public static StaticFee DefaultFee(string accountId, string areaIdentifier)
        {
            return new StaticFee()
            {
                Min = 0,
                Max = 0,
                FeeId = Guid.NewGuid().ToString(),
                AccountId = accountId,
                AreaIdentifier = areaIdentifier
            };
        }

        public static StaticFee BindTemplate(StaticFee oldFee, string accountId, string areaIdentifier)
        {
            return new StaticFee()
            {
                Min = oldFee.Min,
                Max = oldFee.Max,
                FeeId = oldFee.FeeId,
                AccountId = accountId,
                AreaIdentifier = areaIdentifier
            };
        }

        public static StaticFee CreateNew(double min, double max, string feeId, string accountId, string areaIdentifier)
        {
            return new StaticFee()
            {
                Min = min,
                Max = max,
                FeeId = feeId,
                AccountId = accountId,
                AreaIdentifier = areaIdentifier
            };
        }
    }
}