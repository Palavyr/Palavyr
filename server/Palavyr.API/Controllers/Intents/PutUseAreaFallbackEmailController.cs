using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Intents
{

    public class PutUseAreaFallbackEmailController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private readonly IConfigurationRepository configurationRepository;

        public PutUseAreaFallbackEmailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut("intents/use-fallback-email-toggle")]
        public async Task<bool> Put(ModifyUseAreaFallbackEmailRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;

        }
    }
}