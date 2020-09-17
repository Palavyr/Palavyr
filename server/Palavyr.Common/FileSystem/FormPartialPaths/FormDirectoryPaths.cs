using System.IO;
using Palavyr.Common.FileSystem.MagicStrings;

namespace Palavyr.Common.FileSystem.FileSystem.FormDirectoryPaths

{
    public class FormDirectoryPaths
    {

        public static string FormAttachmentDirectory(string accountId, string areaId)
        {
            return Path.Combine(MagicPathStrings.InstallationRoot, MagicPathStrings.UserData, accountId,
                MagicPathStrings.AreaData, areaId, MagicPathStrings.Attachments);
        }
        
    }
}