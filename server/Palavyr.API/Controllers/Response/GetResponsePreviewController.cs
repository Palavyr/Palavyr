using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Response
{
    public class GetResponsePreviewController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "preview/estimate/{intentId}";


        public GetResponsePreviewController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<FileLink> GetConfigurationPreview(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetResponsePreviewRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}