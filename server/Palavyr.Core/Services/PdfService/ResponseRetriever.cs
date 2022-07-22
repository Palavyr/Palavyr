using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PdfService
{
    public class ResponseRetriever<TEntity> : IResponseRetriever<TEntity> where TEntity : class, IEntity, ITable
    {
        private readonly IEntityStore<TEntity> entityStore;
        private readonly IAccountIdTransport accountIdTransport;

        public ResponseRetriever(IEntityStore<TEntity> entityStore, IAccountIdTransport accountIdTransport)
        {
            this.entityStore = entityStore;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<List<TEntity>> RetrieveAllAvailableResponses(string pricingStrategyResponseId)
        {
            return await entityStore
                .RawReadonlyQuery()
                .Where(x => x.AccountId == accountIdTransport.AccountId)
                .Where(x => pricingStrategyResponseId.EndsWith(x.TableId))
                .ToListAsync(entityStore.CancellationToken);
        }
    }
}