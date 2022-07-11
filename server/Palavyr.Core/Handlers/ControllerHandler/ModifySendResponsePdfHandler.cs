﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifySendResponsePdfHandler : IRequestHandler<ModifySendResponseRequest, ModifySendResponseResponse>
    {
        private readonly IEntityStore<Area> intentStore;

        public ModifySendResponsePdfHandler(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifySendResponseResponse> Handle(ModifySendResponseRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            var newState = !area.SendPdfResponse;
            area.SendPdfResponse = newState;
            return new ModifySendResponseResponse(newState);
        }
    }

    public class ModifySendResponseResponse
    {
        public ModifySendResponseResponse(bool newState)
        {
            NewState = newState;
        }

        public bool NewState { get; set; }
    }

    public class ModifySendResponseRequest : IRequest<ModifySendResponseResponse>
    {
        public ModifySendResponseRequest(string intentId)
        {
            IntentId = intentId;
        }

        public static string FormatRoute(string intentId)
        {
            return Route.Replace("{intentId}", intentId);
        }

        public const string Route = "area/send-pdf/{intentId}";
        public string IntentId { get; set; }
    }
}