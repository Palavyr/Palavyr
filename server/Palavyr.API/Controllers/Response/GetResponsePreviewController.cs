using System;
using System.Globalization;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.CommonResponseTypes;
using Palavyr.API.Response;

namespace Palavyr.API.Controllers.Response
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class GetResponsePreviewController : ControllerBase
    {
        private readonly IAccountsConnector accountsConnector;
        private IAmazonS3 s3Client;
        private ILogger<GetResponsePreviewController> logger;
        private readonly IPdfResponseGenerator pdfResponseGenerator;

        public GetResponsePreviewController(
            IAccountsConnector accountsConnector,
            ILogger<GetResponsePreviewController> logger,
            IAmazonS3 s3Client,
            IPdfResponseGenerator pdfResponseGenerator
        )
        {
            this.accountsConnector = accountsConnector;
            this.s3Client = s3Client;
            this.logger = logger;
            this.pdfResponseGenerator = pdfResponseGenerator;
        }

        [HttpGet("preview/estimate/{areaId}")]
        public async Task<IActionResult> GetConfigurationPreview([FromHeader] string accountId, string areaId)
        {
            logger.LogDebug("Attempting to generate a new preview");
            var account = await accountsConnector.GetAccount(accountId);
            var locale = account.Locale;
            var culture = new CultureInfo(locale);

            FileLink fileLink;
            try
            {
                fileLink = await pdfResponseGenerator.CreatePdfResponsePreviewAsync(s3Client, culture, accountId, areaId);
                logger.LogDebug("Successfully created a Response preview!");
                logger.LogDebug($"File Link: {fileLink.Link}");
                logger.LogDebug($"File Id: {fileLink.FileId}");
                logger.LogDebug($"File Name: {fileLink.FileName}");
            }
            catch (Exception e)
            {
                logger.LogDebug($"Failed to Create a preview! Error: {e.Message}");
                return BadRequest();
            }

            return Ok(fileLink);
        }
    }
}