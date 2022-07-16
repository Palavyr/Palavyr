using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Stores;

namespace Palavyr.API.Controllers.Intents
{
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
            var newName = request.IntentName;

            var intent = await intentStore.Get(intentId, s => s.IntentId);
            if (newName != intent.IntentName)
            {
                intent.IntentName = newName;
            }

            return new ModifyIntentNameResponse(newName);
        }
    }


    public class ModifyIntentNameRequest : IRequest<ModifyIntentNameResponse>
    {
        public const string Route = "intents/update/name/{intentId}";

        public static string FormatRoute(string intentId)
        {
            return Route.Replace("{intentId}", intentId);
        }

        public string IntentName { get; set; }
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