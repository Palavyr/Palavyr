﻿using Palavyr.API.Controllers.Accounts.Setup.SeedData;

namespace Palavyr.Data.Setup.SeedData
{
    public class SeedData : BaseSeedData
    {
        public SeedData(string accountId, string defaultEmail) : base(accountId, defaultEmail)
        {
        }
    }
}