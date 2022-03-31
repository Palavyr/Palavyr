using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Mappers;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetCompanyLogoController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/logo";

        public GetCompanyLogoController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<FileAssetResource> Get(
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetCompanyLogoRequest(), cancellationToken);
            return response.Response;
        }
    }
}