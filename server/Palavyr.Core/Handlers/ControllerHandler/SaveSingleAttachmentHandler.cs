using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SaveSingleAttachmentHandler : IRequestHandler<SaveSingleAttachmentRequest, SaveSingleAttachmentResponse>
    {
        private readonly IAttachmentSaver attachmentSaver;
        private readonly IAttachmentRetriever attachmentRetriever;

        public SaveSingleAttachmentHandler(
            IAttachmentSaver attachmentSaver,
            IAttachmentRetriever attachmentRetriever)
        {
            this.attachmentSaver = attachmentSaver;
            this.attachmentRetriever = attachmentRetriever;
        }

        public async Task<SaveSingleAttachmentResponse> Handle(SaveSingleAttachmentRequest request, CancellationToken cancellationToken)
        {
            await attachmentSaver.SaveAttachment(request.IntentId, request.Attachment);
            var attachmentFileLinks = await attachmentRetriever.RetrieveAttachmentLinks(request.IntentId, cancellationToken);
            return new SaveSingleAttachmentResponse(attachmentFileLinks);
        }
    }

    public class SaveSingleAttachmentResponse
    {
        public SaveSingleAttachmentResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class SaveSingleAttachmentRequest : IRequest<SaveSingleAttachmentResponse>
    {
        public SaveSingleAttachmentRequest(string intentId, IFormFile attachment)
        {
            IntentId = intentId;
            Attachment = attachment;
        }

        public string IntentId { get; set; }
        public IFormFile Attachment { get; set; }
    }
}