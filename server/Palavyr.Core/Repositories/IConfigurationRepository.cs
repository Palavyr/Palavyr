using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Repositories
{
    public interface IConfigurationRepository
    {
        Task CommitChangesAsync();
        Task CommitChangesAsync(CancellationToken cancellationToken);
        Task<Area> CreateAndAddNewArea(string name, string accountId, string emailAddress, bool isVerified);
        Task<List<Area>> GetAllAreasShallow(string accountId);
        Task<Area> GetAreaById(string accountId, string areaId);
        Task<ConversationNode> GetConversationNodeById(string nodeId);
        Task<ConversationNode> GetConversationNodeById(string nodeId, string accountId);
        Task<List<ConversationNode>> GetConversationNodeByIds(List<string> nodeIds);

        Task<List<ConversationNode>> GetAreaConversationNodes(string accountId, string areaId);
        Task<List<ConversationNode>> UpdateConversation(string accountId, string areaId, List<ConversationNode> convoUpdate, CancellationToken cancellationToken);
        Task<ConversationNode> UpdateConversationNodeText(string accountId, string areaId, string nodeId, string nodeTextUpdate);

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

        Task<Image> GetImageById(string imageId, CancellationToken cancellationToken);
        Task<Image[]> GetImagesByIds(string[] imageIds, CancellationToken cancellationToken);
        Task<ConversationNode[]> GetConvoNodesByImageIds(string[] imageIds, CancellationToken cancellationToken);
        Task RemoveImagesByIds(string[] imageIds, IS3Deleter s3Deleter, string userDataBucket, CancellationToken cancellationToken);
        Task<Image[]> GetImagesByAccountId(string accountId, CancellationToken cancellationToken);

        // maintenance methods to delete
        Task<List<DynamicTableMeta>> GetAllDynamicTableMetas();
        Task<List<ConversationNode>> GetAllConversationNodes();
    }
}