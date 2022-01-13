﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Conversation
{
    public class GetMissingIntroductionSequenceTreeErrors
    {
        public const string Route = "configure-conversations/intro/tree-errors";
        private readonly IMediator mediator;

        public GetMissingIntroductionSequenceTreeErrors(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task<TreeErrorsResponse> GetIntro(
            [FromBody]
            GetMissingIntroductionSequenceTreeErrorsRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}