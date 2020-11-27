using System.Collections.Generic;
using System.IO;
using System.Linq;
using DashboardServer.Data;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystem.FormPaths;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.Background
{
    public class ValidateAttachments : IValidateAttachments
    {
        private readonly DashContext DashContext;
        private readonly AccountsContext AccountsContext;
        private readonly ILogger<ValidateAttachments> _logger;

        public ValidateAttachments(DashContext dashContext, AccountsContext accountsContext,
            ILogger<ValidateAttachments> logger)
        {
            DashContext = dashContext;
            AccountsContext = accountsContext;
            _logger = logger;
        }

        /// <summary>
        /// Clear the database of any entries where the file name doesn't exist on the file system. Run this Daily or hourly.
        /// </summary>
        public void ValidateAllAttachments()
        {
            var attachments = DashContext.FileNameMaps.ToList();

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

            DashContext.FileNameMaps.RemoveRange(staleDbEntries);
            DashContext.SaveChanges();
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

            var accounts = AccountsContext.Accounts.Select(x => x.AccountId).ToList();
            foreach (var account in accounts)
            {
                // TODO: getting area list from disk? for from db? DB should be the source of truth.
                var areas = DashContext.Areas.Where(row => row.AccountId == account).Select(x => x.AreaIdentifier)
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
                        var record = DashContext.FileNameMaps.SingleOrDefault(row => row.SafeName == fileStem);
                        if (record == null)
                        {
                            // the file exists on disk, but not in the DB
                            var message =
                                $"FILE INCONSISTENCY!! Account: {account} -- Area: {areaIdentifier} -- FileStem: {fileInfo.Name}";
                            _logger.LogCritical(message);
                        }
                    }
                }
            }
        }
    }
}