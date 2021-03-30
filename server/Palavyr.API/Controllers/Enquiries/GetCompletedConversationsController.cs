using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.ConversationServices;

namespace Palavyr.API.Controllers.Enquiries
{

    public class GetCompletedConversationsController : PalavyrBaseController
    {
        private readonly ILogger<GetCompletedConversationsController> logger;
        private readonly CompletedConversationRetriever completedConversationRetriever;


        public GetCompletedConversationsController(
            ILogger<GetCompletedConversationsController> logger,
            CompletedConversationRetriever completedConversationRetriever
        )
        {
            this.logger = logger;
            this.completedConversationRetriever = completedConversationRetriever;
        }

        [HttpGet("enquiries")]
        public async Task<Enquiry[]> Get([FromHeader] string accountId)
        {
            return await completedConversationRetriever.RetrieveCompletedConversations(accountId);
        }
    }
}