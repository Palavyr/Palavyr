using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.ConversationServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetEnquiriesHandler : IRequestHandler<GetEnquiriesRequest, GetEnquiriesResponse>
    {
        private readonly IConversationRecordRetriever conversationRecordRetriever;

        public GetEnquiriesHandler(IConversationRecordRetriever conversationRecordRetriever)
        {
            this.conversationRecordRetriever = conversationRecordRetriever;
        }

        public async Task<GetEnquiriesResponse> Handle(GetEnquiriesRequest request, CancellationToken cancellationToken)
        {
            var result = await conversationRecordRetriever.RetrieveConversationRecords();
            return new GetEnquiriesResponse(result);
        }
    }

    public class GetEnquiriesResponse
    {
        public GetEnquiriesResponse(Enquiry[] response) => Response = response;
        public Enquiry[] Response { get; set; }
    }

    public class GetEnquiriesRequest : IRequest<GetEnquiriesResponse>
    {
    }
}