using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.ConversationServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetEnquiriesHandler : IRequestHandler<GetEnquiriesRequest, GetEnquiriesResponse>
    {
        private readonly IConversationRecordRetriever conversationRecordRetriever;
        private readonly IMapToNew<ConversationHistoryMeta, EnquiryResource> mapper;

        public GetEnquiriesHandler(IConversationRecordRetriever conversationRecordRetriever, IMapToNew<ConversationHistoryMeta, EnquiryResource> mapper)
        {
            this.conversationRecordRetriever = conversationRecordRetriever;
            this.mapper = mapper;
        }

        public async Task<GetEnquiriesResponse> Handle(GetEnquiriesRequest request, CancellationToken cancellationToken)
        {
            var records = await conversationRecordRetriever.RetrieveConversationRecords();

            var enquiries = await mapper.MapMany(records, cancellationToken);
            return new GetEnquiriesResponse(enquiries);
        }
    }

    public class PaginatedEnquiryData
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<EnquiryResource> Data { get; set; }
    }

    public class GetEnquiriesResponse
    {
        public GetEnquiriesResponse(IEnumerable<EnquiryResource> response) => Response = response;
        public IEnumerable<EnquiryResource> Response { get; set; }
    }

    public class GetEnquiriesRequest : IRequest<GetEnquiriesResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}