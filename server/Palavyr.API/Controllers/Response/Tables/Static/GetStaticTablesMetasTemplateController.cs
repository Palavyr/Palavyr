using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    public class GetStaticTablesMetasTemplateController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "response/configuration/{intentId}/static/tables/template";


        public GetStaticTablesMetasTemplateController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpGet(Route)]
        public async Task<StaticTablesMeta> CreateNewStaticTablesMeta(string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetStaticTablesMetasTemplateRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}