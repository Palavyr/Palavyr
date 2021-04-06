using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Repositories
{
    public interface IConfigurationRepository
    {
        Task CommitChangesAsync();
        Task<Area> CreateAndAddNewArea(string name, string accountId, string emailAddress, bool isVerified);
        Task<List<Area>> GetAllAreasShallow(string accountId);
        Task<Area> GetAreaById(string accountId, string areaId);
        Task<List<ConversationNode>> GetAreaConversationNodes(string accountId, string areaId);
        Task<ConversationNode> GetConversationNodeById(string nodeId);
        Task<List<ConversationNode>> GetConversationNodeByIds(List<string> nodeIds);
        Task<Area> GetAreaWithConversationNodes(string accountId, string areaId);
        Task RemoveConversationNodeById(string nodeId);
        Task<List<ConversationNode>> UpdateConversationNode(string accountId, string areaId, string nodeId, ConversationNode newNode);
        void RemoveNodeRangeByIds(List<string> nodeIds);
        Task<Area> GetAreaComplete(string accountId, string areaId);
        Task<List<StaticTablesMeta>> GetStaticTables(string accountId, string areaId);
        Task<WidgetPreference> GetWidgetPreferences(string accountId);
        Task<List<Area>> GetActiveAreas(string accountId);
        Task SetDefaultDynamicTable(string accountId, string areaId, string tableId);
        Task RemoveStaticTables(List<StaticTablesMeta> staticTablesMetas);
        Task<List<Area>> GetActiveAreasWithConvoAndDynamicAndStaticTables(string accountId);
        void RemoveAreaNodes(string areaId, string accountId);
        
        Task<List<DynamicTableMeta>> GetDynamicTableMetas(string accountId, string areaIdentifier);
        Task<DynamicTableMeta> GetDynamicTableMetaByTableId(string tableId);
        
        
        // maintenance methods to delete
        Task<List<DynamicTableMeta>> GetAllDynamicTableMetas();
        Task<List<ConversationNode>> GetAllConversationNodes();
    }
}