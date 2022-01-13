using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts
{
    public class DeleteAccountController : PalavyrBaseController
    {
        public const string Route = "account/delete-account";
        public DeleteAccountController()
        {
        }

        [HttpPost(Route)]
        public async Task DeleteAccount(CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteAccountRequest(), cancellationToken);
        }
    }
}