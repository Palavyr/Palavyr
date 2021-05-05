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

        [HttpDelete("enquiries/{fileId}")]
        public async Task<Enquiry[]> Delete(
            [FromHeader]
            string accountId,
            [FromRoute]
            string fileId,
            CancellationToken cancellationToken)
        {
            await enquiryDeleter.DeleteEnquiry(accountId, fileId, cancellationToken);

            // get the new enquiry list and return it
            return await completedConversationRetriever.RetrieveCompletedConversations(accountId);
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