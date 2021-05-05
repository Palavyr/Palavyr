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
        private readonly ICompletedConversationRetriever completedConversationRetriever;

        public GetEnquiriesController(
            ILogger<GetEnquiriesController> logger,
            ICompletedConversationRetriever completedConversationRetriever
        )
        {
            this.logger = logger;
            this.completedConversationRetriever = completedConversationRetriever;
        }

        [HttpGet("enquiries")]
        public async Task<Enquiry[]> Get(
            [FromHeader]
            string accountId)
        {
            return await completedConversationRetriever.RetrieveCompletedConversations(accountId);
        }
    }
}