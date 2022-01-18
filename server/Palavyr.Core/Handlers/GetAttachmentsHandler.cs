using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.Core.Handlers
{
    public class GetAttachmentsHandler : IRequestHandler<GetAttachmentsRequest, GetAttachmentsResponse>
    {
        private readonly IAttachmentRetriever attachmentRetriever;

        public GetAttachmentsHandler(IAttachmentRetriever attachmentRetriever)
        {
            this.attachmentRetriever = attachmentRetriever;
        }

        public async Task<GetAttachmentsResponse> Handle(GetAttachmentsRequest request, CancellationToken cancellationToken)
        {
            var attachmentFileLinks = await attachmentRetriever.RetrieveAttachmentLinks(request.IntentId, cancellationToken);
            return new GetAttachmentsResponse(attachmentFileLinks);
        }
    }

    public class GetAttachmentsResponse
    {
        public GetAttachmentsResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class GetAttachmentsRequest : IRequest<GetAttachmentsResponse>
    {
        public GetAttachmentsRequest(string intentId)
        {
            IntentId = intentId;
            
        }

        public string IntentId { get; set; }
    }
}