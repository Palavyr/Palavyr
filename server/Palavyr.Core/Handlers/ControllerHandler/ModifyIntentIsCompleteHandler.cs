using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyIntentIsCompleteHandler : IRequestHandler<ModifyIntentIsCompleteRequest, ModifyIntentIsCompleteResponse>
    {
        private readonly IEntityStore<Area> intentStore;

        public ModifyIntentIsCompleteHandler(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyIntentIsCompleteResponse> Handle(ModifyIntentIsCompleteRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            area.IsEnabled = request.IsEnabled;
            return new ModifyIntentIsCompleteResponse(area.IsEnabled);
        }
    }

    public class ModifyIntentIsCompleteResponse
    {
        public ModifyIntentIsCompleteResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ModifyIntentIsCompleteRequest : IRequest<ModifyIntentIsCompleteResponse>
    {
        public string IntentId { get; set; }
        public bool IsEnabled { get; set; }
    }
}