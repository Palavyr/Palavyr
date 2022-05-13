using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources.Responses;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetPhoneNumberController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/phone-number";

        public GetPhoneNumberController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<PhoneDetailsResource> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetPhoneNumberRequest(), cancellationToken);
            return response.Response;
        }
    }
}

