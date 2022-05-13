﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic
{
    public class GetIntroductionNodeTypeOptionsController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public GetIntroductionNodeTypeOptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public const string Route = "configure-intro/{introId}/node-type-options";

        [HttpGet(Route)]
        public async Task<IEnumerable<NodeTypeOptionResource>> GetIntro([FromRoute] string introId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetIntroductionNodeTypeOptionsRequest(introId), cancellationToken);
            return response.Response;
        }
    }
}