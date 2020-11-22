using Palavyr.API.Controllers;

namespace Palavyr.API.controllers.accounts.seedData
{
    public class SeedData : BaseSeedData
    {
        public SeedData(string accountId, string defaultEmail) : base(accountId, defaultEmail)
        {
        }
    }
}