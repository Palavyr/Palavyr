using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.ImageServices
{
    public class ImageRemover : IImageRemover
    {
        private readonly IConfigurationRepository repository;
        private readonly IS3Deleter s3Deleter;
        private readonly IConfiguration configuration;

        public ImageRemover(IConfigurationRepository repository, IS3Deleter s3Deleter, IConfiguration configuration)
        {
            this.repository = repository;
            this.s3Deleter = s3Deleter;
            this.configuration = configuration;
        }

        public async Task<FileLink[]> RemoveImages(string[] ids, CancellationToken cancellationToken)
        {
            await repository.RemoveImagesByIds(ids, s3Deleter, configuration.GetUserDataBucket());
            var convoNodesReferencingImageId = await repository.GetConvoNodesByImageIds(ids);
            foreach (var node in convoNodesReferencingImageId)
            {
                node.ImageId = null;
            }

            await repository.CommitChangesAsync();
            var links = await repository.GetImagesByAccountId();
            return links.ToFileLinks();
        }
    }
}