using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts
{
    public class DeleteAccountController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/delete-account";

        public DeleteAccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task DeleteAccount(CancellationToken cancellationToken)
        {
            await mediator.Publish(new DeleteAccountNotification(), cancellationToken);
        }
    }
}