using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Accounts
{
    public class CancelRegistrationController : PalavyrBaseController
    {
        public const string Route = "account/cancel-registration";

        private readonly IMediator mediator;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IEntityStore<Account> accountStore;

        public CancelRegistrationController(IMediator mediator, IAccountIdTransport accountIdTransport, IEntityStore<Account> accountStore)
        {
            this.mediator = mediator;
            this.accountIdTransport = accountIdTransport;
            this.accountStore = accountStore;
        }

        [AllowAnonymous]
        [HttpPost(Route)]
        public async Task Post(CancelRegistrationNotification notification, CancellationToken cancellationToken)
        {
            var account = await accountStore.Get(notification.EmailAddress, s => s.EmailAddress);
            if (account.Active) throw new DomainException("Can not delete an active account");
            var accountId = account.AccountId;


            accountIdTransport.Assign(accountId);
            await mediator.Publish(notification, cancellationToken);
        }
    }
}