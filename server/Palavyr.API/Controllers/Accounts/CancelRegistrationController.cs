using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Accounts
{
    public class CancelRegistrationController : PalavyrBaseController
    {
        public const string Route = "account/cancel-registration";

        private readonly IMediator mediator;
        private readonly IHoldAnAccountId accountIdHolder;
        private readonly IAccountRepository accountRepository;

        public CancelRegistrationController(IMediator mediator, IHoldAnAccountId accountIdHolder, IAccountRepository accountRepository)
        {
            this.mediator = mediator;
            this.accountIdHolder = accountIdHolder;
            this.accountRepository = accountRepository;
        }

        [AllowAnonymous]
        [HttpPost(Route)]
        public async Task Post(CancelRegistrationNotification notification, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccountByEmailAddressOrNull(notification.EmailAddress);
            if (account.Active) throw new DomainException("Can not delete an active account");
            var accountId = account.AccountId;


            accountIdHolder.Assign(accountId);
            await mediator.Publish(notification, cancellationToken);
        }
    }
}