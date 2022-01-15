using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Authentication
{
    public class CreateLoginRequestController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "authentication/login";

        public CreateLoginRequestController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        // https://codeburst.io/jwt-auth-in-asp-net-core-148fb72bed03
        [AllowAnonymous]
        [HttpPost(Route)]
        public async Task<Credentials> RequestLogin([FromBody] CreateLoginRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}