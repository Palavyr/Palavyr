﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetResponseConfigurationHandler : IRequestHandler<GetResponseConfigurationRequest, GetResponseConfigurationResponse>
    {
        private readonly IEntityStore<Area> intentStore;

        public GetResponseConfigurationHandler(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetResponseConfigurationResponse> Handle(GetResponseConfigurationRequest request, CancellationToken cancellationToken)
        {
            var areaWithAllData = await intentStore.GetIntentComplete(request.IntentId);
            return new GetResponseConfigurationResponse(areaWithAllData);
        }
    }

    public class GetResponseConfigurationResponse
    {
        public GetResponseConfigurationResponse(Area response) => Response = response;
        public Area Response { get; set; }
    }

    public class GetResponseConfigurationRequest : IRequest<GetResponseConfigurationResponse>
    {
        public GetResponseConfigurationRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}