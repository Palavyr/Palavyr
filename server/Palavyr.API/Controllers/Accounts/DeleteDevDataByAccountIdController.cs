using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts
{
    public class DeleteDevDataByAccountIdController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "dev/{devKey}/{accountId}";

        public DeleteDevDataByAccountIdController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpDelete(Route)]
        public async Task Delete(string accountId, string devKey, CancellationToken cancellationToken)
        {
            await mediator.Publish(new DeleteDevDataByAccountIdRequest(accountId, devKey), cancellationToken);
        }
    }
}