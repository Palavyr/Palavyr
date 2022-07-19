using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Services.PdfService
{
    public interface IResponseRetriever<TEntity> where TEntity : class, IEntity, ITable
    {
        Task<List<TEntity>> RetrieveAllAvailableResponses(string pricingStrategyResponseId);
    }
}