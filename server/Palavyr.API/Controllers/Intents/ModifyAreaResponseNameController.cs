using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Intents
{
    public class ModifyIntentNameController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public ModifyIntentNameController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        public class IntentNameBody
        {
            public string IntentName { get; set; }
        }

        [HttpPut(ModifyIntentNameRequest.Route)]
        public async Task<string> Modify(
            [FromBody]
            IntentNameBody intentNameText,
            string intentId,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(new ModifyIntentNameRequest(), cancellationToken);
            return response.IntentName;
        }
    }
}