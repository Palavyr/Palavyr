using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

// TODO Move this to docs somewhere

// DESIGN for the DTO from the widget. The node id refers to the node set in the conversationNOde table. this has a 'resolveOrder' property which is used to
// determine the order in which these responses should be used to resolve the final result.
// [
//     {
//         "DynamicTableKey?": [
//             {[node.nodeId]: "Response Value"}, 1
//             {[node.nodeId]: "Response Value"}, 2
//             {[node.nodeId]: "Response Value"}  0
//         ],
//          "SecondPartPossibly?": [
//              {"node.nodeId"}
//         ]
//     },
//     {
//         "SelectOneFlat-1231": [
//             {"SelectOneFlat-1231": "Ruby"}
//         ]
//     }
// ]


namespace Palavyr.Core.Services.DynamicTableService
{
    public abstract class BaseCompiler<TEntity> where TEntity : class, IDynamicTable<TEntity>
    {
        protected readonly IGenericDynamicTableRepository<TEntity> Repository;

        public BaseCompiler(IGenericDynamicTableRepository<TEntity> repository)
        {
            Repository = repository;
        }

        protected async Task<List<TEntity>> GetTableRows(DynamicTableMeta dynamicTableMeta)
        {
            var (accountId, areaId, tableId) = dynamicTableMeta;
            var rows = await Repository.GetAllRows(accountId, areaId, tableId);
            return rows;
        }

        protected string GetSingleResponseValue(DynamicResponse dynamicResponse, List<string> dynamicResponseIds)
        {
            var responseComponent = dynamicResponse.ResponseComponents[0]; // this expects only a single response;
            var responseValue = responseComponent.Values.ToList()[0];
            return responseValue;
        }

        protected string GetSingleResponseId(List<string> dynamicResponseIds)
        {
            return dynamicResponseIds[0];
        }

        protected string GetResponseByResponseId(string responseId, DynamicResponse dynamicResponse)
        {
            return dynamicResponse.ResponseComponents.Single(x => x.ContainsKey(responseId)).Values.ToList().Single();
        }

        protected async Task<List<string>> GetResponsesOrderedByResolveOrder(DynamicResponse dynamicResponse)
        {
            var responseKeys = dynamicResponse.ResponseComponents.SelectMany(row => row.Keys).ToList();
            return (await Repository.GetConversationNodeByIds(responseKeys))
                .OrderBy(x => x.ResolveOrder)
                .Select(x => x.NodeId)
                .ToList();
        }
    }
}