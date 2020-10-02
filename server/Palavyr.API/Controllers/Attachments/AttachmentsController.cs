using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.ResponseTypes;
using Palavyr.Common.FileSystem.FormPaths;
using Server.Domain.Configuration.schema;


namespace Palavyr.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : BaseController
    {
        private static ILogger<FileController> _logger;
        private IAmazonS3 S3Client { get; set; }
        
        public FileController(
            ILogger<FileController> logger,
            IAmazonS3 s3Client, 
            AccountsContext accountContext, 
            ConvoContext convoContext,
            DashContext dashContext, 
            IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
        {
            S3Client = s3Client;
            _logger = logger;
        }

        /// <summary>
        /// CURRENLTY IN USE
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{areaId}")]
        public async Task<FileLink[]> GetAttachmentLinksAsUri([FromHeader] string accountId, string areaId)
        {
            if (accountId == null)
                throw new Exception();

            var files = AttachmentPaths.GetAttachmentFileList(accountId, areaId);

            var links = new List<FileLink>();
            foreach (var fi in files)
            {
                var fileMap = DashContext.FileNameMaps.Single(row => row.SafeName == fi.Name);
                var link = await UriUtils.CreateAttachmentLinkAsURI(_logger, accountId, areaId, fileMap.SafeName, S3Client);
                links.Add(FileLink.CreateLink(fileMap.RiskyName, link, fileMap.SafeName));
            }
            return links.ToArray();
        }
        
        [HttpDelete("{areaId}/filelink")]
        public Task<FileLink[]> DeleteAttachment(string areaId, [FromHeader] string accountId, Text text)
        {
            var filePath = FormFilePath.FormAttachmentFilePath(accountId, areaId, text.FileId);
            if (DiskUtils.ValidatePathExists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            var entity = DashContext.FileNameMaps.SingleOrDefault(row => row.SafeName == text.FileId);
            if (entity != null)
            {
                DashContext.FileNameMaps.Remove(entity);
                DashContext.SaveChanges();
            }
            
            return GetAttachmentLinksAsUri(accountId, areaId);
        }

        
        [HttpPost("{areaId}/savemany")]
        [ActionName("Decode")]
        public Task<FileLink[]> SaveAttachments(string areaId, [FromHeader] string accountId,
            [FromForm(Name = "files")] IList<IFormFile> attachmentFiles)
        {
            // TODO write filename only to the database, then generate GUID to use as filename, then save, then use the db map of guid to filename to get the file.
            var attachmentDir = FormDirectoryPaths.FormAttachmentDirectoryWithCreate(accountId, areaId);
            foreach (var formFile in attachmentFiles)
            {
                Console.WriteLine(formFile.FileName);
                var safeFileName = Guid.NewGuid().ToString() + ".pdf";
                var riskyFileName = formFile.FileName;

                var fileMap = FileNameMap.CreateFileMap(safeFileName, riskyFileName, accountId, areaId);
                DashContext.FileNameMaps.AddAsync(fileMap);

                var fileSavePath = Path.Combine(attachmentDir, safeFileName);
                using var fileStream = new FileStream(fileSavePath, FileMode.Create);
                formFile.CopyTo(fileStream);
            }

            DashContext.SaveChanges();
            return GetAttachmentLinksAsUri(accountId, areaId);
        }

        [HttpPost("{areaId}/saveone")]
        [ActionName("Decode")]
        public async Task<FileLink[]> SaveAttachment([FromHeader] string accountId, string areaId, [FromForm(Name = "files")] IFormFile attachmentFile)
        {
            var attachmentDir = FormDirectoryPaths.FormAttachmentDirectoryWithCreate(accountId, areaId);
            var safeFileName = Guid.NewGuid() + ".pdf";
            var riskyFileName = attachmentFile.FileName;

            var fileNameMap = FileNameMap.CreateFileMap(safeFileName, riskyFileName, accountId, areaId);

            await DashContext.FileNameMaps.AddAsync(fileNameMap); // must be async
            await DashContext.SaveChangesAsync();

            var fileSavePath = Path.Combine(attachmentDir, safeFileName);
            await using var fileStream = new FileStream(fileSavePath, FileMode.Create);
            await attachmentFile.CopyToAsync(fileStream);
            fileStream.Close();
            
            return await GetAttachmentLinksAsUri(accountId, areaId);
        }
    }
}