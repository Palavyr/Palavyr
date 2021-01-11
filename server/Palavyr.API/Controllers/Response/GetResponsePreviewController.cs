using System;
using System.Globalization;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Response;
using Palavyr.API.ResponseTypes;

namespace Palavyr.API.Controllers.Response
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

        public GetResponsePreviewController(
            ILogger<GetResponsePreviewController> logger, 
            IAmazonS3 s3Client,
            DashContext dashContext,
            AccountsContext accountsContext
        )
        {
            this.s3Client = s3Client;
            this.logger = logger;
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
        }

        [HttpGet("preview/estimate/{areaId}")]
        public async Task<IActionResult> GetConfigurationPreview([FromHeader] string accountId, string areaId)
        {
            logger.LogDebug("Attempting to generate a new preview");
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            var locale = account.Locale;
            var culture = new CultureInfo(locale);
            
            var pdfGenerator = new PdfResponseGenerator(dashContext, accountsContext, accountId, areaId, Request, logger);

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