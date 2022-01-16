using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.EnquiryServices;

namespace Palavyr.Core.Handlers
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
        public EnquiryInsightsResponse(EnquiryInsightsResource[] response) => Response = response;
        public EnquiryInsightsResource[] Response { get; set; }
    }

    public class EnquiryInsightsRequest : IRequest<EnquiryInsightsResponse>
    {
    }
}