using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ConversationServices;
using Palavyr.Core.Services.EnquiryServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteEnquiryHandler : IRequestHandler<DeleteEnquiryRequest, DeleteEnquiryResponse>
    {
        private readonly IEnquiryDeleter enquiryDeleter;
        private readonly IConversationRecordRetriever conversationRecordRetriever;

        public DeleteEnquiryHandler(
            IEnquiryDeleter enquiryDeleter,
            IConversationRecordRetriever conversationRecordRetriever)
        {
            this.enquiryDeleter = enquiryDeleter;
            this.conversationRecordRetriever = conversationRecordRetriever;
        }

        public async Task<DeleteEnquiryResponse> Handle(DeleteEnquiryRequest request, CancellationToken cancellationToken)
        {
            await enquiryDeleter.DeleteEnquiries(request.FileReferences, cancellationToken);
            var result = await conversationRecordRetriever.RetrieveConversationRecords();
            return new DeleteEnquiryResponse(result);
        }
    }

    public class DeleteEnquiryResponse
    {
        public DeleteEnquiryResponse(Enquiry[] response) => Response = response;
        public Enquiry[] Response { get; set; }
    }

    public class DeleteEnquiryRequest : IRequest<DeleteEnquiryResponse>
    {
        public string[] FileReferences { get; set; }
    }
}