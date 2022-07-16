using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Mappers;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetResponsePreviewHandler : IRequestHandler<GetResponsePreviewRequest, GetResponsePreviewResponse>
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly ILogger<GetResponsePreviewHandler> logger;
        private readonly IMapToNew<FileAsset, FileAssetResource> resourceMapper;
        private readonly IPreviewResponseGenerator previewPdfGenerator;

        public GetResponsePreviewHandler(
            IEntityStore<Account> accountStore,
            ILogger<GetResponsePreviewHandler> logger,
            IMapToNew<FileAsset, FileAssetResource> resourceMapper,
            IPreviewResponseGenerator previewPdfGenerator)
        {
            this.accountStore = accountStore;
            this.logger = logger;
            this.resourceMapper = resourceMapper;
            this.previewPdfGenerator = previewPdfGenerator;
        }

        public async Task<GetResponsePreviewResponse> Handle(GetResponsePreviewRequest request, CancellationToken cancellationToken)
        {
            var culture = await accountStore.GetCulture();

            try
            {
                var fileAsset = await previewPdfGenerator.CreatePdfResponsePreviewAsync(request.IntentId, culture);
                var resource = await resourceMapper.Map(fileAsset);

                return new GetResponsePreviewResponse(resource);
            }
            catch (Exception e)
            {
                logger.LogDebug("Failed to Create a preview! Error: {ErrorMessage}", e.Message);
                throw new DomainException(e.Message);
            }
        }
    }

    public class GetResponsePreviewResponse
    {
        public GetResponsePreviewResponse(FileAssetResource response) => Response = response;
        public FileAssetResource Response { get; set; }
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