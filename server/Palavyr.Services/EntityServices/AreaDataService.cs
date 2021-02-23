using System.Linq;
using Microsoft.EntityFrameworkCore;
using Palavyr.Data;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Services.EntityServices
{
    public interface IAreaDataService
    {
        Area GetSingleAreaDataRecursive(string accountId, string areaId);
    }

    public class AreaDataService : IAreaDataService
    {
        private readonly DashContext dashContext;

        public AreaDataService(DashContext dashContext)
        {
            this.dashContext = dashContext;
        }
        
        public Area GetSingleAreaDataRecursive(string accountId, string areaId)
        {
            var areaData = dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee)
                .Single(row => row.AreaIdentifier == areaId);
            return areaData;
        }
    }
}