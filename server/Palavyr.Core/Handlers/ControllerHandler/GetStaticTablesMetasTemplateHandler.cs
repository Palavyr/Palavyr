using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetStaticTablesMetasTemplateHandler : IRequestHandler<GetStaticTablesMetasTemplateRequest, GetStaticTablesMetasTemplateResponse>
    {
        private readonly ILogger<GetStaticTablesMetasTemplateHandler> logger;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IMapToNew<StaticTablesMeta, StaticTableMetaResource> mapper;

        public GetStaticTablesMetasTemplateHandler(
            ILogger<GetStaticTablesMetasTemplateHandler> logger,
            IAccountIdTransport accountIdTransport,
            IMapToNew<StaticTablesMeta, StaticTableMetaResource> mapper)
        {
            this.logger = logger;
            this.accountIdTransport = accountIdTransport;
            this.mapper = mapper;
        }

        public async Task<GetStaticTablesMetasTemplateResponse> Handle(GetStaticTablesMetasTemplateRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var resource = await mapper.Map(StaticTablesMeta.CreateNewMetaTemplate(request.IntentId, accountIdTransport.AccountId));
            return new GetStaticTablesMetasTemplateResponse(resource);
        }
    }

    public class GetStaticTablesMetasTemplateResponse
    {
        public GetStaticTablesMetasTemplateResponse(StaticTableMetaResource response) => Response = response;
        public StaticTableMetaResource Response { get; set; }
    }

    public class GetStaticTablesMetasTemplateRequest : IRequest<GetStaticTablesMetasTemplateResponse>
    {
        public GetStaticTablesMetasTemplateRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}