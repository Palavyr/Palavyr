using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Accounts
{
    public class DeleteAccountController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public DeleteAccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpDelete(DeleteAccountRequest.Route)]
        public async Task DeleteAccount(CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteAccountRequest(), cancellationToken);
        }
    }
}