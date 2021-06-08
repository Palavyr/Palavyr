using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly DashContext dashContext;
        private readonly ILogger<ConfigurationRepository> logger;

        public ConfigurationRepository(DashContext dashContext, ILogger<ConfigurationRepository> logger)
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        public async Task<List<DynamicTableMeta>> GetDynamicTableMetas(string accountId, string areaIdentifier)
        {
            return await dashContext
                .DynamicTableMetas
                .Where(row => row.AccountId == accountId && row.AreaIdentifier == areaIdentifier)
                .ToListAsync();
        }

        public async Task<DynamicTableMeta> GetDynamicTableMetaByTableId(string tableId)
        {
            return await dashContext
                .DynamicTableMetas
                .Where(row => row.TableId == tableId)
                .SingleAsync();
        }

        public async Task<Image> GetImageById(string imageId, CancellationToken cancellationToken)
        {
            // validate the image id
            var image = await dashContext.Images.SingleOrDefaultAsync(x => x.ImageId == imageId, cancellationToken);
            if (image == null)
            {
                throw new DomainException("Image Id was not found");
            }

            return image;
        }

        public async Task<Image[]> GetImagesByIds(string[] imageIds, CancellationToken cancellationToken)
        {
            var images = await dashContext.Images.Where(x => imageIds.Contains(x.ImageId)).ToArrayAsync(cancellationToken);
            return images;
        }

        public async Task<ConversationNode[]> GetConvoNodesByImageIds(string[] imageIds, CancellationToken cancellationToken)
        {
            var convoNodes = await dashContext.ConversationNodes.Where(x => imageIds.Contains(x.ImageId)).ToArrayAsync(cancellationToken); // this could be empty
            return convoNodes;
        }

        public async Task RemoveImagesByIds(string[] imageIds, IS3Deleter s3Deleter, string userDataBucket, CancellationToken cancellationToken)
        {
            var images = dashContext.Images.Where(x => imageIds.Contains(x.ImageId));

            var s3Keys = new List<string>();
            foreach (var image in images)
            {
                s3Keys.Add(image.S3Key);
            }

            var result = await s3Deleter.DeleteObjectsFromS3Async(userDataBucket, s3Keys.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            if (!result)
            {
                throw new DomainException("Could not delete files from S3!");
            }

            dashContext.RemoveRange(images);
        }

        public async Task<Image[]> GetImagesByAccountId(string accountId, CancellationToken cancellationToken)
        {
            var images = await dashContext.Images.Where(x => x.AccountId == accountId).ToArrayAsync(cancellationToken);
            return images;
        }

        public async Task CommitChangesAsync()
        {
            await dashContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            await dashContext.SaveChangesAsync(cancellationToken);
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

        public async Task<List<ConversationNode>> GetConversationNodeByIds(List<string> nodeIds)
        {
            logger.LogDebug($"Retrieving Conversation Node {nodeIds}");
            var result = await dashContext
                .ConversationNodes
                .Where(row => nodeIds.Contains(row.NodeId))
                .ToListAsync();
            return result;
        }

        public async Task<List<ConversationNode>> UpdateConversation(string accountId, string areaId, List<ConversationNode> convoUpdate, CancellationToken cancellationToken)
        {
            var area = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(p => p.ConversationNodes)
                .SingleOrDefaultAsync();

            RemoveAreaNodes(areaId, accountId);
            await dashContext.SaveChangesAsync(cancellationToken);

            area.ConversationNodes.AddRange(convoUpdate);
            await dashContext.SaveChangesAsync(cancellationToken);
            return convoUpdate;
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
                newNode.NodeComponentType,
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

        public void RemoveAreaNodes(string areaId, string accountId)
        {
            dashContext.ConversationNodes.RemoveRange(dashContext.ConversationNodes.Where(row => row.AccountId == accountId && row.AreaIdentifier == areaId));
        }

        public async Task<List<DynamicTableMeta>> GetAllDynamicTableMetas()
        {
            return await dashContext.DynamicTableMetas.ToListAsync();
        }

        public async Task<List<ConversationNode>> GetAllConversationNodes()
        {
            return await dashContext.ConversationNodes.ToListAsync();
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
                .Where(row => row.AccountId == accountId && row.IsEnabled)
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
            return await dashContext.Areas.Where(row => row.AccountId == accountId && row.IsEnabled).ToListAsync();
        }

        public async Task SetDefaultDynamicTable(string accountId, string areaId, string tableId)
        {
            var defaultDynamicTable = new SelectOneFlat();
            var defaultTable = defaultDynamicTable.CreateTemplate(accountId, areaId, tableId);
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