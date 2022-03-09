﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ModifyAreaFallbackEmailSubjectHandler : IRequestHandler<ModifyAreaFallbackEmailSubjectRequest, ModifyAreaFallbackEmailSubjectResponse>
    {
        private readonly IConfigurationEntityStore<Area> intentStore;

        public ModifyAreaFallbackEmailSubjectHandler(IConfigurationEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<ModifyAreaFallbackEmailSubjectResponse> Handle(ModifyAreaFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var curArea = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);

            if (request.Subject != curArea.FallbackSubject)
            {
                curArea.FallbackSubject = request.Subject;
            }

            return new ModifyAreaFallbackEmailSubjectResponse(curArea.FallbackSubject);
        }
    }

    public class ModifyAreaFallbackEmailSubjectResponse
    {
        public ModifyAreaFallbackEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class ModifyAreaFallbackEmailSubjectRequest : IRequest<ModifyAreaFallbackEmailSubjectResponse>
    {
        public string Subject { get; set; }
        public string IntentId { get; set; }
    }
}