using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetStaticTableRowTemplateHandler : IRequestHandler<GetStaticTableRowTemplateRequest, GetStaticTableRowTemplateResponse>
    {
        private readonly IAccountIdTransport accountIdTransport;

        public GetStaticTableRowTemplateHandler(IAccountIdTransport accountIdTransport)
        {
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<GetStaticTableRowTemplateResponse> Handle(GetStaticTableRowTemplateRequest request, CancellationToken cancellationToken)
        {
            return new GetStaticTableRowTemplateResponse(StaticTableRow.CreateStaticTableRowTemplate(int.Parse(request.TableId), request.IntentId, accountIdTransport.AccountId));

        }
    }

    public class GetStaticTableRowTemplateResponse
    {
        public GetStaticTableRowTemplateResponse(StaticTableRow response) => Response = response;
        public StaticTableRow Response { get; set; }
    }

    public class GetStaticTableRowTemplateRequest : IRequest<GetStaticTableRowTemplateResponse>
    {
        public GetStaticTableRowTemplateRequest(string intentId, string tableId)
        {
            IntentId = intentId;
            TableId = tableId;
        }
        public string IntentId { get; set; }
        public string TableId { get; set; }

    }
}