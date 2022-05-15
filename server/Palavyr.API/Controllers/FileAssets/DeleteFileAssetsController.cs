using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Mappers;

namespace Palavyr.API.Controllers.FileAssets
{
    public class DeleteFileAssetsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private readonly IGuidFinder guidFinder;
        private const string Route = "file-assets";

        public DeleteFileAssetsController(IMediator mediator, IGuidFinder guidFinder)
        {
            this.mediator = mediator;
            this.guidFinder = guidFinder;
        }

        [HttpDelete(Route)]
        public async Task<IEnumerable<FileAssetResource>> DeleteImageById(
            [FromQuery]
            string fileIds,
            CancellationToken cancellationToken
        )
        {
            var ids = fileIds?.Split(",") ?? new string[] { };

            // ids should be guids
            foreach (var id in ids)
            {
                // This throws if a GUID is not found.
                guidFinder.FindFirstGuidSuffixOrNull(id);
            }
            
            var response = await mediator.Send(new DeleteImagesRequest(ids), cancellationToken);
            return response.Response;
        }
    }
}