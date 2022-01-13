using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetApiKeyController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Uri = "account/settings/api-key";

        public GetApiKeyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Uri)]
        public async Task<string> Get()
        {
            var response = await mediator.Send(new GetApiKeyRequest());
            return response.Response;
        }
    }
}