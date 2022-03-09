﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetCompanyNameHandler : IRequestHandler<GetCompanyNameRequest, GetCompanyNameResponse>
    {
        private readonly IConfigurationEntityStore<Account> accountStore;

        public GetCompanyNameHandler(
            IConfigurationEntityStore<Account> accountStore)
        {
            this.accountStore = accountStore;
        }

        public async Task<GetCompanyNameResponse> Handle(GetCompanyNameRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            return new GetCompanyNameResponse(account.CompanyName ?? "");
        }
    }

    public class GetCompanyNameResponse
    {
        public GetCompanyNameResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetCompanyNameRequest : IRequest<GetCompanyNameResponse>
    {
    }
}