using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Palavyr.API.Controllers.Accounts
{
    public class DeleteAccountController : PalavyrBaseController
    {
        public DeleteAccountController()
        {
        }

        [HttpPost("account/delete-account")]
        public async Task DeleteAccount(CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteAccountRequest(), cancellationToken);
        }
    }
}