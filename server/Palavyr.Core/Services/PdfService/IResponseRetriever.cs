using System.Collections.Generic;
using System.Threading.Tasks;

namespace Palavyr.Core.Services.PdfService
{
    public interface IResponseRetriever
    {
        Task<List<TEntity>> RetrieveAllAvailableResponses<TEntity>(string accountId, string dynamicResponseId) where TEntity : class;
    }
}