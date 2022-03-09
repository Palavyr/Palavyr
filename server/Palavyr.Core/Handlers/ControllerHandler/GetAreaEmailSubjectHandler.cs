﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetAreaEmailSubjectHandler : IRequestHandler<GetAreaEmailSubjectRequest, GetAreaEmailSubjectResponse>
    {
        private readonly IConfigurationEntityStore<Area> intentStore;

        public GetAreaEmailSubjectHandler(IConfigurationEntityStore<Area> intentStore)
        {
            this.intentStore = intentStore;
        }

        public async Task<GetAreaEmailSubjectResponse> Handle(GetAreaEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var area = await intentStore.Get(request.IntentId, s => s.AreaIdentifier);
            var subject = area.Subject;
            return new GetAreaEmailSubjectResponse(subject);
        }
    }

    public class GetAreaEmailSubjectResponse
    {
        public GetAreaEmailSubjectResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetAreaEmailSubjectRequest : IRequest<GetAreaEmailSubjectResponse>
    {
        public GetAreaEmailSubjectRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}