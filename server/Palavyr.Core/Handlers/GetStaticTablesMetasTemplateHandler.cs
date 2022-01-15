using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Handlers
{
    public class GetStaticTablesMetasTemplateHandler : IRequestHandler<GetStaticTablesMetasTemplateRequest, GetStaticTablesMetasTemplateResponse>
    {
        private readonly ILogger<GetStaticTablesMetasTemplateHandler> logger;
        private readonly IHoldAnAccountId accountIdHolder;

        public GetStaticTablesMetasTemplateHandler(ILogger<GetStaticTablesMetasTemplateHandler> logger, IHoldAnAccountId accountIdHolder)
        {
            this.logger = logger;
            this.accountIdHolder = accountIdHolder;
        }

        public async Task<GetStaticTablesMetasTemplateResponse> Handle(GetStaticTablesMetasTemplateRequest request, CancellationToken cancellationToken)
        {
            return new GetStaticTablesMetasTemplateResponse(StaticTablesMeta.CreateNewMetaTemplate(request.IntentId, accountIdHolder.AccountId));
        }
    }

    public class GetStaticTablesMetasTemplateResponse
    {
        public GetStaticTablesMetasTemplateResponse(StaticTablesMeta response) => Response = response;
        public StaticTablesMeta Response { get; set; }
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