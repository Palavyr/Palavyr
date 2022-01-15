﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Areas
{
    public class ModifyShowDynamicTableTotalsController : PalavyrBaseController
    {
        public const string Route = "area/dynamic-totals";
        private readonly IMediator mediator;

        public ModifyShowDynamicTableTotalsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<bool> Post(
            ModifyShowDynamicTableTotalsRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}