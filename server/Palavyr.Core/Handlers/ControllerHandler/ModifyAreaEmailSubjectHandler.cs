using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyAreaEmailSubjectHandler : IRequestHandler<ModifyAreaEmailSubjectRequest, ModifyAreaEmailSubjectResponse>
    {
        private readonly IEntityStore<Area> intentStore;

        public ModifyAreaEmailSubjectHandler(IEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyAreaEmailSubjectResponse> Handle(ModifyAreaEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var curArea = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            if (request.Subject != curArea.Subject)
            {
                curArea.Subject = request.Subject;
            }

            return new ModifyAreaEmailSubjectResponse(request.Subject);
        }
    }

    public class ModifyAreaEmailSubjectResponse
    {
        public ModifyAreaEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyAreaEmailSubjectRequest : IRequest<ModifyAreaEmailSubjectResponse>
    {
        public const string Route = "email/subject";

        public string Subject { get; set; }
        public string IntentId { get; set; }
    }
}