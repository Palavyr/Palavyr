﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.Localization;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetCurrentLocalAndLocaleMapHandler : IRequestHandler<GetCurrentLocalAndLocaleMapRequest, GetCurrentLocalAndLocaleMapResponse>
    {
        private readonly ICurrentLocaleAndLocaleMapRetriever currentLocaleAndLocaleMapRetriever;

        public GetCurrentLocalAndLocaleMapHandler(ICurrentLocaleAndLocaleMapRetriever currentLocaleAndLocaleMapRetriever)
        {
            this.currentLocaleAndLocaleMapRetriever = currentLocaleAndLocaleMapRetriever;
        }

        public async Task<GetCurrentLocalAndLocaleMapResponse> Handle(GetCurrentLocalAndLocaleMapRequest request, CancellationToken cancellationToken)
        {
            var localeDetails = await currentLocaleAndLocaleMapRetriever.GetLocaleDetails(request.Read);
            return new GetCurrentLocalAndLocaleMapResponse(localeDetails);
        }
    }

    public class GetCurrentLocalAndLocaleMapResponse
    {
        public GetCurrentLocalAndLocaleMapResponse(LocaleMetaResource resource) => Resource = resource;
        public LocaleMetaResource Resource { get; set; }
    }

    public class GetCurrentLocalAndLocaleMapRequest : IRequest<GetCurrentLocalAndLocaleMapResponse>
    {
        public bool Read { get; set; }
    }
}