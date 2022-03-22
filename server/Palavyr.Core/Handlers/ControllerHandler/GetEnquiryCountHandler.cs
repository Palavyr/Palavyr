using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.ConversationServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetEnquiryCountHandler : IRequestHandler<GetEnquiryCountRequest, GetEnquiryCountResponse>
    {
        private readonly IConversationRecordRetriever conversationRecordRetriever;

        public GetEnquiryCountHandler(IConversationRecordRetriever conversationRecordRetriever)
        {
            this.conversationRecordRetriever = conversationRecordRetriever;
        }

        public async Task<GetEnquiryCountResponse> Handle(GetEnquiryCountRequest request, CancellationToken cancellationToken)
        {
            var count = await conversationRecordRetriever.GetActiveEnquiryCount();
            return new GetEnquiryCountResponse(count);
        }
    }

    public class GetEnquiryCountRequest : IRequest<GetEnquiryCountResponse>
    {
    }

    public class GetEnquiryCountResponse
    {
        public GetEnquiryCountResponse(int response) => Response = response;
        public int Response { get; set; }
    }
}