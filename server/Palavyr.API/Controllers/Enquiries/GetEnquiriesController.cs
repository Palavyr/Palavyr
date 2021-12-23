using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ConversationServices;

namespace Palavyr.API.Controllers.Enquiries
{
    public class GetEnquiriesController : PalavyrBaseController
    {
        private readonly ILogger<GetEnquiriesController> logger;
        private readonly IConversationRecordRetriever conversationRecordRetriever;

        public GetEnquiriesController(
            ILogger<GetEnquiriesController> logger,
            IConversationRecordRetriever conversationRecordRetriever
        )
        {
            this.logger = logger;
            this.conversationRecordRetriever = conversationRecordRetriever;
        }

        [HttpGet("enquiries")]
        public async Task<Enquiry[]> Get()
        {
            return await conversationRecordRetriever.RetrieveConversationRecords();
        }
    }
}