#nullable enable
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Conversation.Images
{
    public class GetFileAssetsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private readonly IGuidFinder guidFinder;
        private const string Route = "file-assets";

        public GetFileAssetsController(IMediator mediator, IGuidFinder guidFinder)
        {
            this.mediator = mediator;
            this.guidFinder = guidFinder;
        }

        [HttpGet(Route)]
        public async Task<FileAsset[]> Action([FromQuery] string? fileIds, CancellationToken cancellationToken)
        {
            var ids = fileIds?.Split(',') ?? new string[] { };
            if (fileIds != null)
            {
                foreach (var id in ids)
                {
                    guidFinder.AssertGuidPreset(id);
                }
            }

            var response = await mediator.Send(new GetFileAssetsRequest(ids), cancellationToken);
            return response.Response;
        }
    }
}