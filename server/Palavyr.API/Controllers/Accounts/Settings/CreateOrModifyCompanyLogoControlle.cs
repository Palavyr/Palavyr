﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class DeleteCompanyLogoController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/logo";

        public DeleteCompanyLogoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpDelete(Route)]
        public async Task Delete(CancellationToken cancellationToken)
        {
            await mediator.Publish(new DeleteCompanyLogoRequest(), cancellationToken);
        }
    }
}