using System.Collections.Generic;

namespace Palavyr.Core.Services.EnquiryServices
{
    public class EnquiryInsightsResource
    {
        public string IntentName { get; set; }
        public string IntentIdentifier { get; set; }
        public int NumRecords { get; set; }
        public int SentEmailCount { get; set; }
        public int Completed { get; set; }
        public double AverageIntentCompletion { get; set; }
        public List<double> IntentCompletePerIntent { get; set; }
    }
}