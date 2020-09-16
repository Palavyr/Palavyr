using System;
using System.Collections.Generic;
using System.IO;

namespace Palavyr.FileSystem
{
    
    /// <summary>
    ///  Here we enshrine the directory structure of Palavyr
    /// </summary>
    public static class PathFormUtils
    {
        /// <summary>
        ///  Eg. C:\PalavyrData\UserData\{accountId}\AreaData\{areaId}\Attachments\{someFile.pdf}
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="areaId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FormFullAttachmentPath(string accountId, string areaId, string fileName)
        {
            return Path.Combine(
                MagicPathStrings.InstallationRoot, 
                MagicPathStrings.UserData, 
                accountId, 
                MagicPathStrings.AreaData, 
                areaId, 
                MagicPathStrings.Attachments,
                fileName);
        }

        /// <summary>
        ///  E.g. C:\\ConvoBuilderUserData\\UserData\\${accountId}\\PreviewPDF\\${safeFileName}.pdf
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="safeFileNameStem"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static string FormFullResponsePreviewLocalPath(string accountId, string safeFileNameStem, string fileType = "pdf")
        {
            return Path.Combine(
                MagicPathStrings.InstallationRoot,
                MagicPathStrings.UserData,
                accountId,
                MagicPathStrings.PreviewPDF,
                string.Join('.', new List<string>() {safeFileNameStem, fileType})
            );
        }

        /// <summary>
        ///  E.G $"C:\\PalavyrData\\UserData\\{accountId}\\ResponsePDF\\{safeFileName}.pdf";
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="safeFileNameStem"></param>
        /// <param name="filetype"></param>
        /// <returns></returns>
        public static string FormFullResponseLocalPath(string accountId, string safeFileNameStem, string fileType = "pdf")
        {
            return Path.Combine(
                MagicPathStrings.InstallationRoot,
                MagicPathStrings.UserData,
                accountId,
                MagicPathStrings.ResponsePDF,
                string.Join('.', new List<string>() {safeFileNameStem, fileType})
            );
        }
    }
}