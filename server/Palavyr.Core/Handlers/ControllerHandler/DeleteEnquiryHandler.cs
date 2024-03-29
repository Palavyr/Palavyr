﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.ConversationServices;
using Palavyr.Core.Services.EnquiryServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class DeleteEnquiryHandler : IRequestHandler<DeleteEnquiryRequest, DeleteEnquiryResponse>
    {
        private readonly IEnquiryDeleter enquiryDeleter;
        private readonly IMapToNew<ConversationHistoryMeta, EnquiryResource> mapper;
        private readonly IConversationRecordRetriever conversationRecordRetriever;

        public DeleteEnquiryHandler(
            IEnquiryDeleter enquiryDeleter,
            IMapToNew<ConversationHistoryMeta, EnquiryResource> mapper,
            IConversationRecordRetriever conversationRecordRetriever)
        {
            this.enquiryDeleter = enquiryDeleter;
            this.mapper = mapper;
            this.conversationRecordRetriever = conversationRecordRetriever;
        }

        public async Task<DeleteEnquiryResponse> Handle(DeleteEnquiryRequest request, CancellationToken cancellationToken)
        {
            await enquiryDeleter.DeleteEnquiries(request.ConversationIds);
            var records = await conversationRecordRetriever.RetrieveConversationRecords();

            bool FilterRecentlyDeleted(ConversationHistoryMeta r)
            {
                return !request.ConversationIds.Contains(r.ConversationId);
            }

            var filtered = records.Where(r => FilterRecentlyDeleted(r));

            var enquiries = await mapper.MapMany(filtered, cancellationToken);
            return new DeleteEnquiryResponse(enquiries);
        }
    }

    public class DeleteEnquiryResponse
    {
        public DeleteEnquiryResponse(IEnumerable<EnquiryResource> response) => Response = response;
        public IEnumerable<EnquiryResource> Response { get; set; }
    }

    public class DeleteEnquiryRequest : IRequest<DeleteEnquiryResponse>
    {
        public string[] ConversationIds { get; set; }
    }
}