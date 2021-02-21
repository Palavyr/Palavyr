using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;

namespace DashboardServer.Data.Abstractions
{
    public interface IDashConnector
    {
        Task CommitChangesAsync();
        Task<Area> CreateAndAddNewArea(string name, string accountId, string emailAddress, bool isVerified);
        Task<List<Area>> GetAllAreasShallow(string accountId);
        Task<Area> GetAreaById(string accountId, string areaId);
        Task<List<ConversationNode>> GetAreaConversationNodes(string accountId, string areaId);
        Task<ConversationNode> GetConversationNodeById(string nodeId);
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

    }

    public class DashConnector : IDashConnector
    {
        private readonly DashContext dashContext;
        private readonly ILogger<DashConnector> logger;

        public DashConnector(DashContext dashContext, ILogger<DashConnector> logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        public async Task CommitChangesAsync()
        {
            await dashContext.SaveChangesAsync();
        }

        public async Task<Area> CreateAndAddNewArea(string name, string accountId, string emailAddress, bool isVerified)
        {
            logger.LogInformation($"Creating new area for account: {accountId} called {name}");
            var defaultAreaTemplate = Area.CreateNewArea(name, accountId, emailAddress, isVerified);
            var newArea = await dashContext.Areas.AddAsync(defaultAreaTemplate);
            return newArea.Entity;
        }

        public async Task<List<Area>> GetAllAreasShallow(string accountId)
        {
            return await dashContext.Areas.Where(row => row.AccountId == accountId).ToListAsync();
        }

        public async Task<Area> GetAreaById(string accountId, string areaId)
        {
            return await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleAsync(row => row.AreaIdentifier == areaId);
        }

        public async Task<List<ConversationNode>> GetAreaConversationNodes(string accountId, string areaId)
        {
            var conversation = await dashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToListAsync();
            return conversation;
        }

        public async Task<ConversationNode> GetConversationNodeById(string nodeId)
        {
            logger.LogDebug($"Retrieving Conversation Node {nodeId}");
            var result = await dashContext
                .ConversationNodes
                .SingleAsync(row => row.NodeId == nodeId);
            return result;
        }

        public async Task<Area> GetAreaWithConversationNodes(string accountId, string areaId)
        {
            var area = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(p => p.ConversationNodes)
                .SingleOrDefaultAsync();
            return area;
        }

        public async Task RemoveConversationNodeById(string nodeId)
        {
            var toRemove = await GetConversationNodeById(nodeId);
            dashContext.ConversationNodes.RemoveRange(toRemove);
        }

        public async Task<List<ConversationNode>> UpdateConversationNode(string accountId, string areaId, string nodeId, ConversationNode newNode)
        {
            await RemoveConversationNodeById(nodeId);
            var area = await GetAreaWithConversationNodes(accountId, areaId);
            
            var updatedConversation = area
                .ConversationNodes
                .Where(row => row.NodeId != nodeId)
                .ToList();

            var updatedNode = ConversationNode.CreateNew(
                newNode.NodeId,
                newNode.NodeType,
                newNode.Text,
                newNode.AreaIdentifier,
                newNode.NodeChildrenString,
                newNode.OptionPath,
                newNode.ValueOptions,
                accountId,
                newNode.IsRoot,
                newNode.IsCritical,
                newNode.IsMultiOptionType,
                newNode.IsTerminalType
            );

            updatedConversation.Add(updatedNode);
            area.ConversationNodes = updatedConversation;
            return updatedConversation;
        }

        public void RemoveNodeRangeByIds(List<string> nodeIds)
        {
            var nodesToDelete = dashContext
                .ConversationNodes
                .Where(row => nodeIds.Contains(row.NodeId));
            dashContext.ConversationNodes.RemoveRange(nodesToDelete);
        }

        public async Task<Area> GetAreaComplete(string accountId, string areaId)
        {
            var areaData = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee)
                .SingleAsync(row => row.AreaIdentifier == areaId);
            return areaData;
        }

        public async Task<List<Area>> GetActiveAreasWithConvoAndDynamicAndStaticTables(string accountId)
        {
            return await dashContext
                .Areas
                .Where(row => row.AccountId == accountId && row.IsComplete)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(row => row.StaticTableRows)
                .ToListAsync();
        }

        public async Task<List<StaticTablesMeta>> GetStaticTables(string accountId, string areaId)
        {
            var staticTables = await dashContext
                .StaticTablesMetas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(row => row.StaticTableRows)
                .ThenInclude(x => x.Fee)
                .ToListAsync();
            return staticTables;
        }

        public async Task<WidgetPreference> GetWidgetPreferences(string accountId)
        {
            logger.LogDebug($"Getting widget preferences for {accountId}");
            return await dashContext.WidgetPreferences.SingleOrDefaultAsync(row => row.AccountId == accountId);
        }

        public async Task<List<Area>> GetActiveAreas(string accountId)
        {
            return await dashContext.Areas.Where(row => row.AccountId == accountId && row.IsComplete).ToListAsync();
        }

        public async Task SetDefaultDynamicTable(string accountId, string areaId, string tableId)
        {
            var defaultTable = SelectOneFlat.CreateTemplate(accountId, areaId, tableId);
            await dashContext.SelectOneFlats.AddAsync(defaultTable);
        }

        public async Task RemoveStaticTables(List<StaticTablesMeta> staticTablesMetas)
        {
            foreach (var meta in staticTablesMetas)
            {
                foreach (var row in meta.StaticTableRows)
                {
                    dashContext.StaticFees.Remove(await dashContext.StaticFees.FindAsync(row.Fee.Id));
                    dashContext.StaticTablesRows.Remove(await dashContext.StaticTablesRows.FindAsync(row.Id));
                }

                dashContext.StaticTablesMetas.Remove(await dashContext.StaticTablesMetas.FindAsync(meta.Id));
            }

        }
    }
}