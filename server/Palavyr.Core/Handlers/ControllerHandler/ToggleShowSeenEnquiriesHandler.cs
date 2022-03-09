﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class ToggleShowSeenEnquiriesHandler : IRequestHandler<ToggleShowSeenEnquiriesRequest, ToggleShowSeenEnquiriesResponse>
    {
        private readonly IConfigurationEntityStore<Account> accountStore;

        public ToggleShowSeenEnquiriesHandler(IConfigurationEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<ToggleShowSeenEnquiriesResponse> Handle(ToggleShowSeenEnquiriesRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();

            var newValue = !account.ShowSeenEnquiries;
            account.ShowSeenEnquiries = newValue;
            
            return new ToggleShowSeenEnquiriesResponse(newValue);
        }
    }

    public class ToggleShowSeenEnquiriesResponse
    {
        public ToggleShowSeenEnquiriesResponse(bool response) => Response = response;
        public bool Response { get; set; }
    }

    public class ToggleShowSeenEnquiriesRequest : IRequest<ToggleShowSeenEnquiriesResponse>
    {
    }
}