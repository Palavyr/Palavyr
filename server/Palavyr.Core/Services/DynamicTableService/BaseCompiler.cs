using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Stores;


namespace Palavyr.Core.Services.DynamicTableService
{
    public abstract class BaseCompiler<TEntity> where TEntity : class, IDynamicTable<TEntity>
    {
        protected readonly IPricingStrategyEntityStore<TEntity> repository;

        public BaseCompiler(IPricingStrategyEntityStore<TEntity> repository)
        {
            this.repository = repository;
        }

        protected async Task<List<TEntity>> GetTableRows(DynamicTableMeta dynamicTableMeta)
        {
            var (areaId, tableId) = dynamicTableMeta;
            var rows = (await repository.GetAllRows(areaId, tableId)).ToList();

            var indexArray = new List<int> { };
            var orderedEntities = new List<TEntity>() { };

            var success = true;
            foreach (var row in rows)
            {
                if (row is IOrderedRow orderedRow)
                {
                    indexArray.Add(orderedRow.RowOrder);
                }
                else
                {
                    success = false;
                    break;
                }
            }

            if (indexArray.Distinct().Count() != rows.Count()) // defensive in case we forget to add rowOrder correctly...
            {
                return rows;
            }

            foreach (var index in indexArray)
            {
                orderedEntities.Add(rows[index]);
            }

            return success ? orderedEntities : rows;
        }

        protected string GetSingleResponseValue(DynamicResponseParts dynamicResponses, List<string> dynamicResponseIds)
        {
            var responseComponent = dynamicResponses[0]; // this expects only a single response;
            var responseValue = responseComponent.Values.ToList()[0];
            return responseValue;
        }

        protected string GetSingleResponseId(List<string> dynamicResponseIds)
        {
            return dynamicResponseIds[0];
        }

        protected string GetResponseByResponseId(string responseId, DynamicResponseParts dynamicResponse)
        {
            return dynamicResponse.Single(x => x.ContainsKey(responseId)).Values.ToList().Single();
        }

        protected async Task<List<string>> GetResponsesOrderedByResolveOrder(DynamicResponseParts dynamicResponseParts)
        {
            var responseKeys = dynamicResponseParts.SelectMany(row => row.Keys).ToList();
            var nodes = await repository.GetConversationNodeByIds(responseKeys);
            var sorted = nodes.OrderBy(x => x.ResolveOrder)
                    .Select(x => x.NodeId)
                    .ToList();
            return sorted;
        }

        protected async Task<List<TEntity>> GetAllRowsMatchingResponseId(string dynamicResponseId)
        {
            return (await repository.GetAllRowsMatchingDynamicResponseId(dynamicResponseId)).ToList();
        }
    }
}