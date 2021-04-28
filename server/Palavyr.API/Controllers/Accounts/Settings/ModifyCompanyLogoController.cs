using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.FormPaths;
using Palavyr.Core.Common.FileSystemTools.IO;
using Palavyr.Core.Common.FileSystemTools.ListPaths;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class ModifyCompanyLogoController : PalavyrBaseController
    {
        private readonly IConfiguration configuration;
        private ILogger<ModifyCompanyLogoController> logger;
        private AccountsContext accountsContext;
        private readonly ILinkCreator linkCreator;

        public ModifyCompanyLogoController(
            IConfiguration configuration,
            AccountsContext accountsContext,
            ILinkCreator linkCreator,
            ILogger<ModifyCompanyLogoController> logger
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.linkCreator = linkCreator;
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
            await FileIo.SaveFile(filepath, file);
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            account.AccountLogoUri = filepath;
            await accountsContext.SaveChangesAsync();

            var link = await linkCreator.CreateLogoImageLinkAsUri(
                logger,
                accountId,
                Path.GetFileName(filepath),
                filepath,
                previewBucket
            );
            return Ok(link);
        }
    }
}