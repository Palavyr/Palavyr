using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers
{
    public class SaveAsPreExistingFileHandler : INotificationHandler<SaveAsPreExistingFileRequest>
    {
        private readonly IConfigurationRepository repository;

        public SaveAsPreExistingFileHandler(IConfigurationRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(SaveAsPreExistingFileRequest request, CancellationToken cancellationToken)
        {
            // asserts this image exists
            var image = await repository.GetImageById(request.ImageId);
            var convoNode = await repository.GetConversationNodeById(request.NodeId);

            if (convoNode != null)
            {
                convoNode.ImageId = image.ImageId;
                await repository.CommitChangesAsync();
            }
        }
    }


    public class SaveAsPreExistingFileRequest : INotification
    {
        public SaveAsPreExistingFileRequest(string imageId, string nodeId)
        {
            ImageId = imageId;
            NodeId = nodeId;
        }
        public string ImageId { get; set; }
        public string NodeId { get; set; }
    }
}