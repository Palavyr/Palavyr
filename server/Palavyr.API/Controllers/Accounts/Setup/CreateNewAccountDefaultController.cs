using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    public class CreateNewAccountDefaultController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public CreateNewAccountDefaultController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost(CreateNewAccountRequest.Route)]
        public async Task<CredentialsResource> Create(
            [FromBody]
            CreateNewAccountRequest request,
            CancellationToken cancellationToken)
        {
            
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}