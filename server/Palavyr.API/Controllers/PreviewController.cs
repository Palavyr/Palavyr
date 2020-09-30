using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Palavyr.API.GeneratePdf;
using Palavyr.API.ResponseTypes;


namespace Palavyr.API.Controllers
{
    
    [Route("api/preview")]
    [ApiController]
    public class PreviewController : BaseController
    {
        private IAmazonS3 S3Client { get; set; }
        private static ILogger<PreviewController> _logger;

        public PreviewController(
            ILogger<PreviewController> logger, 
            IAmazonS3 amazonS3, 
            AccountsContext accountContext, 
            ConvoContext convoContext,
            DashContext dashContext, 
            IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
            S3Client = amazonS3;
            _logger = logger;
        }

        [HttpGet("estimate/{areaId}")]
        public async Task<FileLink> GetConfigurationPreview([FromHeader] string accountId, string areaId)
        {
            _logger.LogDebug("Attempting to generate a new preview");
            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            var locale = account.Locale;
            var culture = new CultureInfo(locale);
            
            var pdfGenerator = new PdfResponseGenerator(DashContext, AccountContext, ConvoContext, accountId, areaId, Request, _logger);
            return await pdfGenerator.CreatePdfResponsePreviewAsync(S3Client, culture);
        }
    }
}