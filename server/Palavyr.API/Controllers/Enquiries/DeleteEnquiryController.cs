using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ConversationServices;

namespace Palavyr.API.Controllers.Enquiries
{
    public class DeleteEnquiryController : PalavyrBaseController
    {
        private readonly IEnquiryDeleter enquiryDeleter;
        private readonly ICompletedConversationRetriever completedConversationRetriever;

        public DeleteEnquiryController(
            IEnquiryDeleter enquiryDeleter,
            ICompletedConversationRetriever completedConversationRetriever
        )
        {
            this.enquiryDeleter = enquiryDeleter;
            this.completedConversationRetriever = completedConversationRetriever;
        }

        [HttpPut("enquiries/selected")]
        public async Task<Enquiry[]> DeleteSelected(
            [FromHeader] string accountId,
            DeleteEnquiriesRequest request,
            CancellationToken cancellationToken)
        {
            await enquiryDeleter.DeleteEnquiries(accountId, request.FileReferences, cancellationToken);
            return await completedConversationRetriever.RetrieveCompletedConversations(accountId);
        }
    }

    public class DeleteEnquiriesRequest
    {
        public string[] FileReferences { get; set; }
    }
}