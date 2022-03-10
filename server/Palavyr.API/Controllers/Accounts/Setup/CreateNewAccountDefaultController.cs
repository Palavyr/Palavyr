using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    public class CreateNewAccountDefaultController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/create/default";

        public CreateNewAccountDefaultController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost(Route)]
        public async Task<Credentials> Create(
            [FromBody]
            CreateNewAccountDefaultRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}