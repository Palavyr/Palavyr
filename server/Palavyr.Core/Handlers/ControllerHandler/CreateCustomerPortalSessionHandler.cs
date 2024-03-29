﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.StripeServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateCustomerPortalSessionHandler : IRequestHandler<CreateCustomerPortalSessionRequest, CreateCustomerPortalSessionResponse>
    {
        private readonly IStripeCustomerManagementPortalService stripeCustomerManagementPortalService;

        public CreateCustomerPortalSessionHandler(IStripeCustomerManagementPortalService stripeCustomerManagementPortalService)
        {
            this.stripeCustomerManagementPortalService = stripeCustomerManagementPortalService;
        }

        public async Task<CreateCustomerPortalSessionResponse> Handle(CreateCustomerPortalSessionRequest request, CancellationToken cancellationToken)
        {
            var portalUrl = await stripeCustomerManagementPortalService.FormCustomerSubscriptionManagementPortalUrl(request.CustomerId, request.ReturnUrl);
            return new CreateCustomerPortalSessionResponse(portalUrl);
        }
    }

    public class CreateCustomerPortalSessionResponse
    {
        public CreateCustomerPortalSessionResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class CreateCustomerPortalSessionRequest : IRequest<CreateCustomerPortalSessionResponse>
    {
        public string CustomerId { get; set; }
        public string ReturnUrl { get; set; }
    }
}