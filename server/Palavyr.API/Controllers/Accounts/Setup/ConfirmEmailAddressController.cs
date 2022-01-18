using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts.Setup
{
    public class ConfirmEmailAddressController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/confirmation/{authToken}/action/setup";


        public ConfirmEmailAddressController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<bool> Post([FromRoute] string authToken, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ConfirmEmailAddressRequest(authToken), cancellationToken);
            return response.Response;
        }
    }
}