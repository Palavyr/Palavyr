using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ImageServices;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class DeleteImagesController : PalavyrBaseController
    {
        private readonly IImageRemover imageRemover;
        private readonly GuidFinder guidFinder;
        private const string Route = "images";

        public DeleteImagesController(IImageRemover imageRemover, GuidFinder guidFinder)
        {
            this.imageRemover = imageRemover;
            this.guidFinder = guidFinder;
        }

        [HttpDelete(Route)]
        public async Task<FileLink[]> DeleteImageById(
            [FromHeader]
            string accountId,
            [FromQuery]
            string imageIds,
            CancellationToken cancellationToken
        )
        {
            // TODO: https://www.strathweb.com/2017/07/customizing-query-string-parameter-binding-in-asp-net-core-mvc/
            if (!Request.QueryString.HasValue)
            {
                throw new DomainException("Image deletion failed. No image id was provided.");
            }

            var ids = imageIds.Split(',');

            // ids should be guids
            foreach (var id in ids)
            {
                // This throws if a GUID is not found.
                guidFinder.FindFirstGuidSuffix(id);
            }

            var fileLinks = await imageRemover.RemoveImages(ids, accountId, cancellationToken);
            return fileLinks;
        }
    }
}