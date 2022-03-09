using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetShowDynamicTotalsHandlerHandler : IRequestHandler<GetShowDynamicTotalsHandlerRequest, GetShowDynamicTotalsHandlerResponse>
    {
        private readonly IConfigurationEntityStore<Area> intentStore;

        public GetShowDynamicTotalsHandlerHandler(IConfigurationEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetShowDynamicTotalsHandlerResponse> Handle(GetShowDynamicTotalsHandlerRequest request, CancellationToken cancellationToken)
        {
            var intent = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            return new GetShowDynamicTotalsHandlerResponse(intent.IncludeDynamicTableTotals);
        }
    }

    public class GetShowDynamicTotalsHandlerResponse
    {
        public GetShowDynamicTotalsHandlerResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class GetShowDynamicTotalsHandlerRequest : IRequest<GetShowDynamicTotalsHandlerResponse>
    {
        public string IntentId { get; set; }

        public GetShowDynamicTotalsHandlerRequest(string intentId)
        {
            IntentId = intentId;
        }
    }
}