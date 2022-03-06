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
        private readonly IAttachmentAssetSaver attachmentAssetSaver;
        private readonly IAttachmentRetriever attachmentRetriever;

        public SaveSingleAttachmentHandler(
            IAttachmentAssetSaver attachmentAssetSaver,
            IAttachmentRetriever attachmentRetriever)
        {
            this.attachmentAssetSaver = attachmentAssetSaver;
            this.attachmentRetriever = attachmentRetriever;
        }

        public async Task<SaveSingleAttachmentResponse> Handle(SaveSingleAttachmentRequest request, CancellationToken cancellationToken)
        {
            await attachmentAssetSaver.SaveFile(request.IntentId, request.Attachment);
            var attachmentFileLinks = await attachmentRetriever.GetAttachmentLinksForIntent(request.IntentId);
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