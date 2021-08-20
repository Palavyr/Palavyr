using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Enquiries
{
    public class EnquiryInsightsController : PalavyrBaseController
    {
        private readonly ILogger<IEnquiryInsightComputer> logger;
        private readonly IEnquiryInsightComputer enquiryInsightComputer;

        public EnquiryInsightsController(
            ILogger<IEnquiryInsightComputer>logger,
            IEnquiryInsightComputer enquiryInsightComputer)
        {
            this.logger = logger;
            this.enquiryInsightComputer = enquiryInsightComputer;
        }

        [HttpGet("enquiry-insights")]
        public async Task<EnquiryInsightsResource[]> Get(
            [FromHeader]
            string accountId)
        {
            return await enquiryInsightComputer.GetEnquiryInsights(accountId);
        }
    }

    public class EnquiryInsightsResource
    {
        public string IntentName { get; set; }
        public string IntentIdentifier { get; set; }
        public int NumRecords { get; set; }
        public int SentEmailCount { get; set; }
        public int Completed { get; set; }
        public long AverageIntentCompletion { get; set; }
        public List<long> IntentCompletePerIntent { get; set; }
    }
}