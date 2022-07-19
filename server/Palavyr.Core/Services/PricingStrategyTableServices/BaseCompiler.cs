using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public abstract class BaseCompiler<TEntity> where TEntity : class, IPricingStrategyTable<TEntity>, IEntity, ITable
    {
        private readonly IEntityStore<TEntity> entityStore;
        private readonly IEntityStore<ConversationNode> convoNodeStore;

        public BaseCompiler(
            IEntityStore<TEntity> entityStore,
            IEntityStore<ConversationNode> convoNodeStore)
        {
            this.entityStore = entityStore;
            this.convoNodeStore = convoNodeStore;
        }

        protected async Task<List<TEntity>> GetTableRows(PricingStrategyTableMeta pricingStrategyTableMeta)
        {
            var (intentId, tableId) = pricingStrategyTableMeta;
            var rows = await entityStore.GetMany(tableId, s => s.TableId);
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

        protected string GetSingleResponseValue(PricingStrategyResponseParts pricingStrategyResponses, List<string> pricingStrategyResponseIds)
        {
            var responseComponent = pricingStrategyResponses[0]; // this expects only a single response;
            var responseValue = responseComponent.Values.ToList()[0];
            return responseValue;
        }

        protected string GetSingleResponseId(List<string> responseIds)
        {
            return responseIds[0];
        }

        protected string GetResponseByResponseId(string responseId, PricingStrategyResponseParts pricingStrategyResponse)
        {
            return pricingStrategyResponse.Single(x => x.ContainsKey(responseId)).Values.ToList().Single();
        }

        protected async Task<List<string>> GetResponsesOrderedByResolveOrder(PricingStrategyResponseParts pricingStrategyResponseParts)
        {
            var responseKeys = pricingStrategyResponseParts.SelectMany(row => row.Keys).ToList();
            var nodes = await convoNodeStore.GetMany(responseKeys, s => s.NodeId);
            var sorted = nodes.OrderBy(x => x.ResolveOrder)
                .Select(x => x.NodeId)
                .ToList();
            return sorted;
        }

        protected async Task<List<TEntity>> GetAllRowsMatchingResponseId(string responseId)
        {
            return await entityStore
                .RawReadonlyQuery()
                .Where(x => responseId.EndsWith(x.TableId))
                .ToListAsync(entityStore.CancellationToken);
        }
    }
}