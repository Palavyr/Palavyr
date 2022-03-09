using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;
using Palavyr.Core.Services.PdfService;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetResponsePreviewHandler : IRequestHandler<GetResponsePreviewRequest, GetResponsePreviewResponse>
    {
        private readonly IConfigurationEntityStore<Account> accountStore;
        private readonly ILogger<GetResponsePreviewHandler> logger;
        private readonly IPreviewResponseGenerator previewPdfGenerator;

        public GetResponsePreviewHandler(
            IConfigurationEntityStore<Account> accountStore,
            ILogger<GetResponsePreviewHandler> logger,
            IPreviewResponseGenerator previewPdfGenerator)
        {
            this.accountStore = accountStore;
            this.logger = logger;
            this.previewPdfGenerator = previewPdfGenerator;
        }

        public async Task<GetResponsePreviewResponse> Handle(GetResponsePreviewRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Attempting to generate a new preview");
            var account = await accountStore.GetAccount();
            var locale = account.Locale;
            var culture = new CultureInfo(locale);

            FileLink fileLink;
            try
            {
                fileLink = await previewPdfGenerator.CreatePdfResponsePreviewAsync(request.IntentId, culture);
                logger.LogDebug("Successfully created a Response preview!");
                logger.LogDebug($"File Link: {fileLink.Link}");
                logger.LogDebug($"File Id: {fileLink.FileId}");
                logger.LogDebug($"File Name: {fileLink.FileName}");
            }
            catch (Exception e)
            {
                logger.LogDebug($"Failed to Create a preview! Error: {e.Message}");
                throw new Exception(e.Message);
            }

            return new GetResponsePreviewResponse(fileLink);
        }
    }

    public class GetResponsePreviewResponse
    {
        public GetResponsePreviewResponse(FileLink response) => Response = response;
        public FileLink Response { get; set; }
    }

    public class GetResponsePreviewRequest : IRequest<GetResponsePreviewResponse>
    {
        public GetResponsePreviewRequest(string intentId)
        {
            IntentId = intentId;
        }

        public string IntentId { get; set; }
    }
}