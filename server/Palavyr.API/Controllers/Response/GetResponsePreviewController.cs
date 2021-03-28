using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.PdfService;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Response
{
    [Authorize]
    public class GetResponsePreviewController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private ILogger<GetResponsePreviewController> logger;
        private readonly IPreviewResponseGenerator previewPdfGenerator;

        public GetResponsePreviewController(
            IAccountRepository accountRepository,
            ILogger<GetResponsePreviewController> logger,
            IPreviewResponseGenerator previewPdfGenerator
        )
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
            this.previewPdfGenerator = previewPdfGenerator;
        }

        [HttpGet("preview/estimate/{areaId}")]
        public async Task<FileLink> GetConfigurationPreview([FromHeader] string accountId, string areaId)
        {
            logger.LogDebug("Attempting to generate a new preview");
            var account = await accountRepository.GetAccount(accountId);
            var locale = account.Locale;
            var culture = new CultureInfo(locale);

            FileLink fileLink;
            try
            {
                fileLink = await previewPdfGenerator.CreatePdfResponsePreviewAsync(accountId, areaId, culture);
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

            return fileLink;
        }
    }
}