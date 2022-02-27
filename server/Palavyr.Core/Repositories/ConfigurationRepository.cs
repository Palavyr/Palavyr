#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly DashContext dashContext;
        private readonly ILogger<ConfigurationRepository> logger;
        private readonly IAccountIdTransport accountIdTransport;
        public readonly ICancellationTokenTransport cancellationTokenTransport;

        public ConfigurationRepository(DashContext dashContext, ILogger<ConfigurationRepository> logger, IAccountIdTransport accountIdTransport, ICancellationTokenTransport cancellationTokenTransport)
        {
            this.dashContext = dashContext;
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
            this.cancellationTokenTransport = cancellationTokenTransport;
        }

        public async Task<List<DynamicTableMeta>> GetDynamicTableMetas(string areaIdentifier)
        {
            return await dashContext
                .DynamicTableMetas
                .Where(row => row.AccountId == accountIdTransport.AccountId && row.AreaIdentifier == areaIdentifier)
                .ToListAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task<DynamicTableMeta> GetDynamicTableMetaByTableId(string tableId)
        {
            return await dashContext
                .DynamicTableMetas
                .Where(row => row.TableId == tableId)
                .SingleAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task<DynamicTableMeta> UpdateDynamicTableMeta(DynamicTableMeta dynamicTableMeta)
        {
            var meta = dashContext.DynamicTableMetas.Update(dynamicTableMeta);
            return meta.Entity;
        }

        public async Task<Image> GetImageById(string imageId)
        {
            // validate the image id
            var image = await dashContext.Images.SingleOrDefaultAsync(x => x.ImageId == imageId, cancellationTokenTransport.CancellationToken);
            if (image == null)
            {
                throw new DomainException("Image Id was not found");
            }

            return image;
        }

        public async Task<Image[]> GetImagesByIds(string[] imageIds)
        {
            var images = await dashContext.Images.Where(x => imageIds.Contains(x.ImageId)).ToArrayAsync(cancellationTokenTransport.CancellationToken);
            return images;
        }

        public async Task<ConversationNode[]> GetConvoNodesByImageIds(string[] imageIds)
        {
            var convoNodes = await dashContext.ConversationNodes.Where(x => imageIds.Contains(x.ImageId)).ToArrayAsync(cancellationTokenTransport.CancellationToken); // this could be empty
            return convoNodes;
        }

        public async Task RemoveImagesByIds(string[] imageIds, IS3Deleter s3Deleter, string userDataBucket)
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
                throw new DomainException("Could not delete files from the server");
            }

            dashContext.RemoveRange(images);
        }

        public async Task<Image[]> GetImagesByAccountId()
        {
            var images = await dashContext.Images.Where(x => x.AccountId == accountIdTransport.AccountId).ToArrayAsync(cancellationTokenTransport.CancellationToken);
            return images;
        }

        public async Task CommitChangesAsync()
        {
            await dashContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task<Area> CreateAndAddNewArea(string name, string emailAddress, bool isVerified)
        {
            logger.LogInformation($"Creating new area for account: {accountIdTransport.AccountId} called {name}");
            var defaultAreaTemplate = Area.CreateNewArea(name, accountIdTransport.AccountId, emailAddress, isVerified);
            var newArea = await dashContext.Areas.AddAsync(defaultAreaTemplate, cancellationTokenTransport.CancellationToken);
            return newArea.Entity;
        }

        public async Task<List<Area>> GetAllAreasShallow()
        {
            return await dashContext.Areas.Where(row => row.AccountId == accountIdTransport.AccountId).ToListAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task<Area> GetAreaById(string areaId)
        {
            return await dashContext
                .Areas
                .Where(row => row.AccountId == accountIdTransport.AccountId)
                .SingleAsync(row => row.AreaIdentifier == areaId, cancellationTokenTransport.CancellationToken);
        }

        public async Task<List<ConversationNode>> GetAreaConversationNodes(string areaId)
        {
            var conversation = await dashContext
                .ConversationNodes
                .Where(row => row.AccountId == accountIdTransport.AccountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToListAsync(cancellationTokenTransport.CancellationToken);
            return conversation;
        }

        public async Task<ConversationNode?> GetConversationNodeById(string nodeId)
        {
            logger.LogDebug($"Retrieving Conversation Node {nodeId}");
            var result = await dashContext
                .ConversationNodes
                .SingleOrDefaultAsync(row => row.NodeId == nodeId && row.AccountId == accountIdTransport.AccountId, cancellationTokenTransport.CancellationToken);
            return result;
        }

        public async Task<List<ConversationNode>> GetConversationNodeByIds(List<string> nodeIds)
        {
            logger.LogDebug($"Retrieving Conversation Node {nodeIds}");
            var result = await dashContext
                .ConversationNodes
                .Where(row => nodeIds.Contains(row.NodeId))
                .ToListAsync(cancellationTokenTransport.CancellationToken);
            return result;
        }

        public async Task CreateIntroductionSequence(List<ConversationNode> newConversation)
        {
            await dashContext
                .ConversationNodes
                .AddRangeAsync(newConversation);
            await dashContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task<ConversationNode[]> UpdateIntroductionSequence(string introId, List<ConversationNode> update)
        {
            var currentIntroduction = dashContext.ConversationNodes.Where(x => x.AreaIdentifier == introId);
            dashContext.ConversationNodes.RemoveRange(currentIntroduction);
            await dashContext.ConversationNodes.AddRangeAsync(update, cancellationTokenTransport.CancellationToken);
            await dashContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);
            return update.ToArray();
        }

        public async Task<ConversationNode[]> GetIntroductionSequence(string introId)
        {
            var currentIntroduction = await dashContext.ConversationNodes.Where(x => x.AreaIdentifier == introId).ToArrayAsync(cancellationTokenTransport.CancellationToken);
            return currentIntroduction;
        }

        public async Task<List<ConversationNode>> UpdateConversation(string areaId, List<ConversationNode> convoUpdate)
        {
            var area = await dashContext
                .Areas
                .Where(row => row.AccountId == accountIdTransport.AccountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(p => p.ConversationNodes)
                .SingleOrDefaultAsync();

            RemoveAreaNodes(areaId);
            await dashContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);

            area.ConversationNodes.AddRange(convoUpdate);
            await dashContext.SaveChangesAsync(cancellationTokenTransport.CancellationToken);
            return convoUpdate;
        }

        public async Task<Area> GetAreaWithConversationNodes(string areaId)
        {
            var area = await dashContext
                .Areas
                .Where(row => row.AccountId == accountIdTransport.AccountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(p => p.ConversationNodes)
                .SingleOrDefaultAsync(cancellationTokenTransport.CancellationToken);
            return area;
        }

        public async Task RemoveConversationNodeById(string nodeId)
        {
            var toRemove = await GetConversationNodeById(nodeId);
            dashContext.ConversationNodes.RemoveRange(toRemove);
        }

        public async Task<ConversationNode?> UpdateConversationNodeText(string areaId, string nodeId, string nodeTextUpdate)
        {
            var node = await GetConversationNodeById(nodeId);
            if (node != null)
            {
                node.Text = nodeTextUpdate;
                await CommitChangesAsync();
            }

            return node;
        }

        public async Task<List<ConversationNode>> UpdateConversationNode(string areaId, string nodeId, ConversationNode newNode)
        {
            await RemoveConversationNodeById(nodeId);
            var area = await GetAreaWithConversationNodes(areaId);

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
                accountIdTransport.AccountId,
                newNode.NodeComponentType,
                newNode.NodeTypeCode,
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

        public void RemoveAreaNodes(string areaId)
        {
            dashContext.ConversationNodes.RemoveRange(dashContext.ConversationNodes.Where(row => row.AccountId == accountIdTransport.AccountId && row.AreaIdentifier == areaId));
        }

        public async Task<List<DynamicTableMeta>> GetAllDynamicTableMetas()
        {
            return await dashContext.DynamicTableMetas.ToListAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task<List<ConversationNode>> GetAllConversationNodes()
        {
            return await dashContext.ConversationNodes.ToListAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task<Area> GetAreaComplete(string areaId)
        {
            var areaData = await dashContext
                .Areas
                .Where(row => row.AccountId == accountIdTransport.AccountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee)
                .SingleAsync(row => row.AreaIdentifier == areaId, cancellationTokenTransport.CancellationToken);
            return areaData;
        }

        public async Task<List<Area>> GetActiveAreasWithConvoAndDynamicAndStaticTables()
        {
            return await dashContext
                .Areas
                .Where(row => row.AccountId == accountIdTransport.AccountId && row.IsEnabled)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(row => row.StaticTableRows)
                .ToListAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task<List<StaticTablesMeta>> GetStaticTables(string areaId)
        {
            var staticTables = await dashContext
                .StaticTablesMetas
                .Where(row => row.AccountId == accountIdTransport.AccountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(row => row.StaticTableRows)
                .ThenInclude(x => x.Fee)
                .ToListAsync(cancellationTokenTransport.CancellationToken);
            return staticTables;
        }

        public async Task<WidgetPreference> GetWidgetPreferences()
        {
            logger.LogDebug($"Getting widget preferences for {accountIdTransport.AccountId}");
            return await dashContext.WidgetPreferences.SingleOrDefaultAsync(row => row.AccountId == accountIdTransport.AccountId);
        }

        public async Task<List<Area>> GetActiveAreas()
        {
            return await dashContext.Areas.Where(row => row.AccountId == accountIdTransport.AccountId && row.IsEnabled).ToListAsync(cancellationTokenTransport.CancellationToken);
        }

        public async Task SetDefaultDynamicTable(string areaId, string tableId)
        {
            var defaultDynamicTable = new SelectOneFlat();
            var defaultTable = defaultDynamicTable.CreateTemplate(accountIdTransport.AccountId, areaId, tableId);
            await dashContext.SelectOneFlats.AddAsync(defaultTable, cancellationTokenTransport.CancellationToken);
        }

        public async Task RemoveStaticTables(List<StaticTablesMeta> staticTablesMetas)
        {
            foreach (var meta in staticTablesMetas)
            {
                foreach (var row in meta.StaticTableRows)
                {
                    var fee = await dashContext.StaticFees.FindAsync(row.Fee.Id);
                    dashContext.StaticFees.Remove(fee);

                    var staticRow = await dashContext.StaticTablesRows.FindAsync(row.Id);
                    dashContext.StaticTablesRows.Remove(staticRow);
                }

                var staticMeta = await dashContext.StaticTablesMetas.FindAsync(meta.Id);
                dashContext.StaticTablesMetas.Remove(staticMeta);
            }
        }
    }
}