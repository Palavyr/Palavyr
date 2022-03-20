using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class LinkFileAssetToNodeHandler : INotificationHandler<LinkFileAttachmentToNodeRequest>
    {
        private readonly IFileAssetLinker<NodeLinker> linker;

        public LinkFileAssetToNodeHandler(IFileAssetLinker<NodeLinker> linker)
        {
            this.linker = linker;
        }

        public async Task Handle(LinkFileAttachmentToNodeRequest request, CancellationToken cancellationToken)
        {
            await linker.Link(request.FileId, request.NodeId);
        }
    }

    public class LinkFileAttachmentToNodeRequest : INotification
    {
        public LinkFileAttachmentToNodeRequest(string fileId, string nodeId)
        {
            FileId = fileId;
            NodeId = nodeId;
        }

        public string FileId { get; set; }
        public string NodeId { get; set; }
    }
}