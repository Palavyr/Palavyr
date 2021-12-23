using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class SaveAsPreExistingFileController : PalavyrBaseController
    {
        private readonly IConfigurationRepository repository;
        private const string Route = "images/pre-existing/{imageId}/{nodeId}";

        public SaveAsPreExistingFileController(IConfigurationRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost(Route)]
        public async Task Post(

            string imageId,
            string nodeId,
            CancellationToken cancellationToken
        )
        {
            // asserts this image exists
            var image = await repository.GetImageById(imageId);
            var convoNode = await repository.GetConversationNodeById(nodeId);

            if (convoNode != null)
            {
                convoNode.ImageId = image.ImageId;
                await repository.CommitChangesAsync();
            }
        }
    }
}