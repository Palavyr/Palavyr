﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.EnquiryServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class EnquiryInsightsHandler : IRequestHandler<EnquiryInsightsRequest, EnquiryInsightsResponse>
    {
        private readonly IEnquiryInsightComputer enquiryInsightComputer;

        public EnquiryInsightsHandler(IEnquiryInsightComputer enquiryInsightComputer)
        {
            this.enquiryInsightComputer = enquiryInsightComputer;
        }

        public async Task<EnquiryInsightsResponse> Handle(EnquiryInsightsRequest request, CancellationToken cancellationToken)
        {
            var result = await enquiryInsightComputer.GetEnquiryInsights();
            return new EnquiryInsightsResponse(result);
        }
    }

    public class EnquiryInsightsResponse
    {
        public EnquiryInsightsResponse(IEnumerable<EnquiryInsightsResource> response) => Response = response;
        public IEnumerable<EnquiryInsightsResource> Response { get; set; }
    }

    public class EnquiryInsightsRequest : IRequest<EnquiryInsightsResponse>
    {
    }
}