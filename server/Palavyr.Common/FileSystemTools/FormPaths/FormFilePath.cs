using System.Collections.Generic;
using System.IO;

namespace Palavyr.Common.FileSystemTools.FormPaths
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
            var attachmentDir = FormDirectoryPaths.FormAttachmentDirectoryWithCreate(accountId, areaId);
            return Path.Combine(
                attachmentDir,
                fileName);
        }

        /// <summary>
        ///  E.g. C:\\PalavyrData\\UserData\\${accountId}\\PreviewPDF\\${safeFileName}.pdf
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="safeFileNameStem"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static string FormResponsePreviewLocalFilePath(string accountId, string safeFileNameStem,
            string fileType = "pdf")
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
        public static string FormResponsePDFFilePath(string accountId, string safeFileNameStem, string fileType = "pdf")
        {
            return Path.Combine(
                FormDirectoryPaths.FormResponsePdfDirWithCreate(accountId),
                safeFileNameStem.EndsWith(".pdf") ? safeFileNameStem : safeFileNameStem + "." + fileType
            );
        }

        /// <summary>
        ///  Creates the PreviewPDF FilePath
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="fileNameWithSuffix"></param>
        /// <returns></returns>
        public static string FormPreviewPDFFilePath(string accountId, string fileNameWithSuffix)
        {
            return Path.Combine(MagicPathStrings.InstallationRoot, MagicPathStrings.UserData, accountId,
                MagicPathStrings.PreviewPDF, fileNameWithSuffix);
        }
    }
}