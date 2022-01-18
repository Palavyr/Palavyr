using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Conversation.Images
{
    [Obsolete]
    public class UseImageUrlController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "images/use-link";

        public UseImageUrlController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPost(Route)]
        public async Task<FileLink[]> SaveImageUrl(
            [FromBody]
            UseImageUrlRequest request,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}