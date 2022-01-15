﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class ModifyIntroductionSequenceController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/intro-id";

        public ModifyIntroductionSequenceController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<ConversationNode[]> Post(
            [FromBody]
            ModifyIntroductionSequenceRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}