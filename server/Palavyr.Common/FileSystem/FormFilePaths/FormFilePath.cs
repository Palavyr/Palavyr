using System;
using System.Collections.Generic;
using System.IO;
using Palavyr.Common.FileSystem.MagicStrings;

namespace Palavyr.Common.FileSystem.FormFilePaths
{
    
    /// <summary>
    ///  Here we enshrine the directory structure of Palavyr
    /// </summary>
    public static class FormFilePath
    {
        /// <summary>
        ///  Eg. C:\PalavyrData\UserData\{accountId}\AreaData\{areaId}\Attachments\{someFile.pdf}
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="areaId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FormAttachmentFilePath(string accountId, string areaId, string fileName)
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
        public static string FormResponsePreviewLocalFilePath(string accountId, string safeFileNameStem, string fileType = "pdf")
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
        public static string FormResponseLocalFilePath(string accountId, string safeFileNameStem, string fileType = "pdf")
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