using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Common.Aws;
using Palavyr.Common.Constants;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.Common.FileSystem.IO;
using Palavyr.Common.FileSystem.ListPaths;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    [Route("api")]
    [ApiController]
    public class ModifyCompanyLogoController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private ILogger<ModifyCompanyLogoController> logger;
        private AccountsContext accountsContext;
        private readonly IAmazonS3 s3Client;

        public ModifyCompanyLogoController(
            IConfiguration configuration,
            IAmazonS3 s3Client,
            AccountsContext accountsContext, 
            ILogger<ModifyCompanyLogoController> logger
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.s3Client = s3Client;
        }
        
        [HttpPut("account/settings/logo")]
        [ActionName("Decode")]
        public async Task<IActionResult> Modify(
            [FromHeader] string accountId,
            [FromForm(Name = "files")] IFormFile file) // will take form data. Check attachments
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;

            // Get the directory where we save the logo images
            var extension = Path.GetExtension(file.FileName);
            var logoDirectory = FormDirectoryPaths.FormLogoImageDir(accountId);

            var files = LogoPaths.ListLogoPathsAsDiskPaths(accountId);
            if (files.Count > 0)
            {
                var dir = new DirectoryInfo(logoDirectory);
                foreach (var fi in dir.GetFiles())
                {
                    fi.Delete();
                }
            }

            var filepath = Path.Combine(logoDirectory, Guid.NewGuid().ToString()) + extension;
            await FileIO.SaveFile(filepath, file);
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            account.AccountLogoUri = filepath;
            await accountsContext.SaveChangesAsync();

            var link = await UriUtils.CreateLogoImageLinkAsURI(
                logger,
                accountId,
                Path.GetFileName(filepath),
                filepath,
                s3Client,
                previewBucket
            );
            return Ok(link);
        }
    }
}