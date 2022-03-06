using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Services.LogoServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class CreateOrModifyCompanyLogoController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/logo";

        private readonly ILogoAssetSaver logoAssetSaver;

        public CreateOrModifyCompanyLogoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        [ActionName("Decode")]
        public async Task<string> Modify(
            [FromForm(Name = "files")]
            IFormFile file,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new CreateCompanyLogoRequest(file), cancellationToken);
            return response.Response;
        }
    }
}