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
        private readonly IAttachmentSaver attachmentSaver;
        private readonly IAttachmentRetriever attachmentRetriever;

        public SaveMultipleAttachmentsHandler(
            IAttachmentSaver attachmentSaver,
            IAttachmentRetriever attachmentRetriever)
        {
            this.attachmentSaver = attachmentSaver;
            this.attachmentRetriever = attachmentRetriever;
        }

        public async Task<SaveMultipleAttachmentsResponse> Handle(SaveMultipleAttachmentsRequest request, CancellationToken cancellationToken)
        {
            foreach (var attachmentFile in request.Attachments)
            {
                await attachmentSaver.SaveAttachment(request.IntentId, attachmentFile);
            }

            var attachmentFileLinks = await attachmentRetriever.RetrieveAttachmentLinks(request.IntentId, cancellationToken);
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