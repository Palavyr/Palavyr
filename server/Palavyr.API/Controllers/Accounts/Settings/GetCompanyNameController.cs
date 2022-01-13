using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetCompanyNameController : PalavyrBaseController
    {
        public const string Route = "account/settings/company-name";
        private readonly IMediator mediator;

        public GetCompanyNameController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetCompanyNameRequest(), cancellationToken);
            return response.Response;
        }
    }
}