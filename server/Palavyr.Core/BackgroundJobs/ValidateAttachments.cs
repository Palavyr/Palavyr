using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.FormPaths;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.BackgroundJobs
{
    public class ValidateAttachments : IValidateAttachments
    {
        private readonly DashContext dashContext;
        private readonly AccountsContext accountsContext;
        private readonly ILogger<ValidateAttachments> logger;

        public ValidateAttachments(DashContext dashContext, AccountsContext accountsContext,
            ILogger<ValidateAttachments> logger)
        {
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.logger = logger;
        }

        /// <summary>
        /// Clear the database of any entries where the file name doesn't exist on the file system. Run this Daily or hourly.
        /// </summary>
        public void ValidateAllAttachments()
        {
            var attachments = dashContext.FileNameMaps.ToList();

            var staleDbEntries = new List<FileNameMap>();
            foreach (var attachment in attachments)
            {
                var diskPath =
                    FormFilePath.FormAttachmentFilePath(attachment.AccountId, attachment.AreaIdentifier, attachment.SafeName);
                if (!File.Exists(diskPath))
                {
                    staleDbEntries.Add(attachment);
                }
            }

            dashContext.FileNameMaps.RemoveRange(staleDbEntries);
            dashContext.SaveChanges();
        }

        /// <summary>
        /// REPORT any files that do not exist in the database. Run this less frequently - weekly.
        /// DO NOT Automate deleting files. WAAYYY too dangerous. Instead, just tell which files don't exist in the db.
        /// </summary>
        public void ValidateAllFiles()
        {
            //1. get list of all accounts
            //2. iterate through all accountIds and get all areas
            //3. for each area, get list of all attachments on disk
            //4. for each attachment name, trim the suffix and perform a DashContext.FileNameMap lookup on the safeFileName
            //5. if the safe file name exists, check it against the area and accountIds

            var accounts = accountsContext.Accounts.Select(x => x.AccountId).ToList();
            foreach (var account in accounts)
            {
                // TODO: getting area list from disk? for from db? DB should be the source of truth.
                var areas = dashContext.Areas.Where(row => row.AccountId == account).Select(x => x.AreaIdentifier)
                    .ToList();
                foreach (var areaIdentifier in areas)
                {
                    var attachmentDir = FormDirectoryPaths.FormAttachmentDirectoryWithCreate(account, areaIdentifier);
                    var dirInfo = new DirectoryInfo(attachmentDir);
                    var dirContents = dirInfo.GetFiles("*.pdf");
                    foreach (var fileInfo in dirContents)
                    {
                        var fileName = fileInfo.Name;
                        var fileStem = Path.GetFileNameWithoutExtension(fileName);
                        var record = dashContext.FileNameMaps.SingleOrDefault(row => row.SafeName == fileStem);
                        if (record == null)
                        {
                            // the file exists on disk, but not in the DB
                            var message =
                                $"FILE INCONSISTENCY!! Account: {account} -- Area: {areaIdentifier} -- FileStem: {fileInfo.Name}";
                            logger.LogCritical(message);
                        }
                    }
                }
            }
        }
    }
}