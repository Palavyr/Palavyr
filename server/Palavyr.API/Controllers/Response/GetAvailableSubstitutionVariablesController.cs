using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Response
{
    public class GetAvailableSubstitutionVariablesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "email/variables";

        public GetAvailableSubstitutionVariablesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<List<ResponseVariable>> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetAvailableSubstitutionVariablesRequest(), cancellationToken);
            return response.Response;
        }
    }
}