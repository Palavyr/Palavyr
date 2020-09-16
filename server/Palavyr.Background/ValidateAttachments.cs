using System.Collections.Generic;
using System.IO;
using System.Linq;
using DashboardServer.Data;
using Palavyr.FileSystem;
using Server.Domain;

namespace Palavyr.Background
{
    public class ValidateAttachments
    {
        private readonly DashContext Context;
        
        public ValidateAttachments(DashContext context)
        {
            Context = context;
        }
    
        /// <summary>
        /// Clear the database of any entries where the file name doesn't exist on the file system. Run this Daily or hourly.
        /// </summary>
        public void ValidateAllAttachments()
        {
            var attachments = Context.FileNameMaps.ToList();

            var staleDbEntries = new List<FileNameMap>();
            foreach (var attachment in attachments)
            {
                var diskPath = PathFormUtils.FormFullAttachmentPath(attachment.AccountId, attachment.AreaId, attachment.SafeName);
                if (!File.Exists(diskPath))
                {
                    staleDbEntries.Add(attachment);
                }
            }
            Context.FileNameMaps.RemoveRange(staleDbEntries);
            Context.SaveChanges();
        }

        /// <summary>
        /// Clear the disk of any files that do not exist in the database. Run this less frequently - daily.
        /// </summary>
        public void ValidateAllFiles()
        {
            
        }
        
        
    }
}