using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AttachmentServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class SaveMultipleAttachmentsHandler : IRequestHandler<SaveMultipleAttachmentsRequest, SaveMultipleAttachmentsResponse>
    {
        private readonly IAttachmentAssetSaver attachmentAssetSaver;
        private readonly IAttachmentRetriever attachmentRetriever;

        public SaveMultipleAttachmentsHandler(
            IAttachmentAssetSaver attachmentAssetSaver,
            IAttachmentRetriever attachmentRetriever)
        {
            this.attachmentAssetSaver = attachmentAssetSaver;
            this.attachmentRetriever = attachmentRetriever;
        }

        public async Task<SaveMultipleAttachmentsResponse> Handle(SaveMultipleAttachmentsRequest request, CancellationToken cancellationToken)
        {
            foreach (var attachmentFile in request.Attachments)
            {
                await attachmentAssetSaver.SaveFile(request.IntentId, attachmentFile);
            }

            var attachmentFileLinks = await attachmentRetriever.GetAttachmentLinksForIntent(request.IntentId);
            return new SaveMultipleAttachmentsResponse(attachmentFileLinks);
        }
    }

    public class SaveMultipleAttachmentsResponse
    {
        public SaveMultipleAttachmentsResponse(FileLink[] response) => Response = response;
        public FileLink[] Response { get; set; }
    }

    public class SaveMultipleAttachmentsRequest : IRequest<SaveMultipleAttachmentsResponse>
    {
        public SaveMultipleAttachmentsRequest(string intentId, IList<IFormFile> attachments)
        {
            Attachments = attachments;
            IntentId = intentId;
        }

        public string IntentId { get; set; }
        public IList<IFormFile> Attachments { get; set; }
    }
}