using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Services.PdfService
{
    public class ResponseRetriever : IResponseRetriever
    {
        private readonly ILifetimeScope lifetimeScope;

        public ResponseRetriever(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public async Task<List<TEntity>> RetrieveAllAvailableResponses<TEntity>(string dynamicResponseId) where TEntity : class
        {
            var repository = (IGenericDynamicTableRepository<TEntity>) lifetimeScope.Resolve(typeof(IGenericDynamicTableRepository<TEntity>));
            var rows = await repository.GetAllRowsMatchingDynamicResponseId(dynamicResponseId);
            return rows;
        }
    }
}