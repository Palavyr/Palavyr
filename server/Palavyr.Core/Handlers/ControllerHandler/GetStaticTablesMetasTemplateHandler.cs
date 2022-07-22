using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetStaticTablesMetasTemplateHandler : IRequestHandler<GetStaticTablesMetasTemplateRequest, GetStaticTablesMetasTemplateResponse>
    {
        private readonly IGuidUtils guidUtils;
        private readonly ILogger<GetStaticTablesMetasTemplateHandler> logger;
        private readonly IMapToNew<StaticTablesMeta, StaticTableMetaResource> mapper;

        public GetStaticTablesMetasTemplateHandler(
            IGuidUtils guidUtils,
            ILogger<GetStaticTablesMetasTemplateHandler> logger,
            IMapToNew<StaticTablesMeta, StaticTableMetaResource> mapper)
        {
            this.guidUtils = guidUtils;
            this.logger = logger;
            this.mapper = mapper;
        }

        public Task<GetStaticTablesMetasTemplateResponse> Handle(GetStaticTablesMetasTemplateRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetStaticTablesMetasTemplateResponse(new StaticTableMetaResource() { TableId = guidUtils.CreateNewId() }));
        }
    }

    public class GetStaticTablesMetasTemplateResponse
    {
        public GetStaticTablesMetasTemplateResponse(StaticTableMetaResource response) => Response = response;
        public StaticTableMetaResource Response { get; set; }
    }

    public class GetStaticTablesMetasTemplateRequest : IRequest<GetStaticTablesMetasTemplateResponse>
    {
        public const string Route = "response/configuration/static/tables/template";

        public GetStaticTablesMetasTemplateRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}