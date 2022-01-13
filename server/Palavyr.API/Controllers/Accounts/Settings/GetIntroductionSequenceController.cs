﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
    public class GetIntroductionSequenceController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/intro-sequence";

        public GetIntroductionSequenceController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<ConversationNode[]> Get()
        {
            var response = await mediator.Send(new GetIntroductionSequenceRequest());
            return response.Response;
        }
    }
}