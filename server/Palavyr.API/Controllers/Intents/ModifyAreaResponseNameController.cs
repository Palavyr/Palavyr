using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Stores;

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
            public string AreaName { get; set; }
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

    public class ModifyIntentNameHandler : IRequestHandler<ModifyIntentNameRequest, ModifyIntentNameResponse>
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly ILogger<ModifyIntentNameHandler> logger;

        public ModifyIntentNameHandler(
            IEntityStore<Intent> intentStore,
            ILogger<ModifyIntentNameHandler> logger)
        {
            this.intentStore = intentStore;
            this.logger = logger;
        }

        public async Task<ModifyIntentNameResponse> Handle(ModifyIntentNameRequest request, CancellationToken cancellationToken)
        {
            var intentId = request.IntentId;
            var newName = request.AreaName;

            var intent = await intentStore.Get(intentId, s => s.AreaIdentifier);
            if (newName != intent.AreaName)
            {
                intent.AreaName = newName;
            }

            return new ModifyIntentNameResponse(newName);
        }
    }


    public class ModifyIntentNameRequest : IRequest<ModifyIntentNameResponse>
    {
        public const string Route = "areas/update/name/{intentId}";

        public static string FormatRoute(string intentId)
        {
            return Route.Replace("{intentId}", intentId);
        }

        public string AreaName { get; set; }
        public string IntentId { get; set; }
    }

    public class ModifyIntentNameResponse
    {
        public string IntentName { get; }

        public ModifyIntentNameResponse(string intentName)
        {
            IntentName = intentName;
        }
    }
}