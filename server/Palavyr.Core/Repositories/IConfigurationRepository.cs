using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Repositories
{
    public interface IConfigurationRepository
    {
        Task CommitChangesAsync();
        Task<Area> CreateAndAddNewArea(string name, string emailAddress, bool isVerified);
        Task<List<Area>> GetAllAreasShallow();
        Task<Area> GetAreaById(string areaId);
        Task<ConversationNode> GetConversationNodeById(string nodeId);
        Task<List<ConversationNode>> GetConversationNodeByIds(List<string> nodeIds);

        Task<List<ConversationNode>> GetAreaConversationNodes(string areaId);
        Task<List<ConversationNode>> UpdateConversation(string areaId, List<ConversationNode> convoUpdate);
        Task<ConversationNode> UpdateConversationNodeText(string areaId, string nodeId, string nodeTextUpdate);
        Task<List<ConversationNode>> UpdateConversationNode(string areaId, string nodeId, ConversationNode newNode);

        Task<Area> GetAreaWithConversationNodes(string areaId);
        Task RemoveConversationNodeById(string nodeId);
        void RemoveNodeRangeByIds(List<string> nodeIds);
        Task<Area> GetAreaComplete(string areaId);
        Task<List<StaticTablesMeta>> GetStaticTables(string areaId);
        Task<WidgetPreference> GetWidgetPreferences();
        Task<List<Area>> GetActiveAreas();
        Task SetDefaultDynamicTable(string areaId, string tableId);
        Task RemoveStaticTables(List<StaticTablesMeta> staticTablesMetas);
        Task<List<Area>> GetActiveAreasWithConvoAndDynamicAndStaticTables();
        void RemoveAreaNodes(string areaId);

        Task<List<DynamicTableMeta>> GetDynamicTableMetas(string areaIdentifier);
        Task<DynamicTableMeta> GetDynamicTableMetaByTableId(string tableId);
        Task<DynamicTableMeta> UpdateDynamicTableMeta(DynamicTableMeta dynamicTableMeta);

        Task<Image> GetImageById(string imageId);
        Task<Image[]> GetImagesByIds(string[] imageIds);
        Task<ConversationNode[]> GetConvoNodesByImageIds(string[] imageIds);
        Task RemoveImagesByIds(string[] imageIds, IS3Deleter s3Deleter, string userDataBucket);
        Task<Image[]> GetImagesByAccountId();

        // maintenance methods to delete
        Task<List<DynamicTableMeta>> GetAllDynamicTableMetas();
        Task<List<ConversationNode>> GetAllConversationNodes();

        Task CreateIntroductionSequence(List<ConversationNode> newConversation);
        Task<ConversationNode[]> UpdateIntroductionSequence(string introId, List<ConversationNode> update);
        Task<ConversationNode[]> GetIntroductionSequence(string introId);
    }
}