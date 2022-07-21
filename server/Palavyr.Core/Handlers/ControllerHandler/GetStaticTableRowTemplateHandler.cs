using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetStaticTableRowTemplateHandler : IRequestHandler<GetStaticTableRowTemplateRequest, GetStaticTableRowTemplateResponse>
    {
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IMapToNew<StaticTableRow, StaticTableRowResource> mapper;

        public GetStaticTableRowTemplateHandler(IAccountIdTransport accountIdTransport, IMapToNew<StaticTableRow, StaticTableRowResource> mapper)
        {
            this.accountIdTransport = accountIdTransport;
            this.mapper = mapper;
        }

        public Task<GetStaticTableRowTemplateResponse> Handle(GetStaticTableRowTemplateRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetStaticTableRowTemplateResponse(new StaticTableRowResource()));
        }
    }

    public class GetStaticTableRowTemplateResponse
    {
        public GetStaticTableRowTemplateResponse(StaticTableRowResource response) => Response = response;
        public StaticTableRowResource Response { get; set; }
    }

    public class GetStaticTableRowTemplateRequest : IRequest<GetStaticTableRowTemplateResponse>
    {
        public GetStaticTableRowTemplateRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}