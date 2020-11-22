using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Palavyr.API.response;
using Palavyr.API.ResponseTypes;


namespace Palavyr.API.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class GetResponsePreviewController : ControllerBase
    {
        private IAmazonS3 s3Client;
        private ILogger<GetResponsePreviewController> logger;
        private DashContext dashContext;
        private AccountsContext accountsContext;
        private ConvoContext convoContext;

        public GetResponsePreviewController(
            ILogger<GetResponsePreviewController> logger, 
            IAmazonS3 s3Client,
            DashContext dashContext,
            AccountsContext accountsContext,
            ConvoContext convoContext
        )
        {
            this.s3Client = s3Client;
            this.logger = logger;
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.convoContext = convoContext;
        }

        [HttpGet("preview/estimate/{areaId}")]
        public async Task<IActionResult> GetConfigurationPreview([FromHeader] string accountId, string areaId)
        {
            logger.LogDebug("Attempting to generate a new preview");
            var account = accountsContext.Accounts.Single(row => row.AccountId == accountId);
            var locale = account.Locale;
            var culture = new CultureInfo(locale);
            
            var pdfGenerator = new PdfResponseGenerator(dashContext, accountsContext, convoContext, accountId, areaId, Request, logger);

            FileLink fileLink;
            try
            {
                fileLink = await pdfGenerator.CreatePdfResponsePreviewAsync(s3Client, culture);
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