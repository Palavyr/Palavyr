using System.IO;
using Microsoft.Extensions.Logging;


namespace Palavyr.API.pathUtils
{
    public static class ResponsePDFPaths
    {
        public static string GetResponsePDFAsDiskPath(ILogger _logger, string accountId, string responsePdfId)
        {
            var userFolder = DiskUtils.GetUserFolderAsDiskPath(accountId);
            _logger.LogInformation("2. User Folder: " + userFolder);
            
            var pdfFile = responsePdfId.EndsWith(".pdf") ? responsePdfId : responsePdfId + ".pdf";

            _logger.LogInformation("3. Pdf File: " + pdfFile);
            
            return Path.Combine(userFolder, MagicString.ResponsePDF, pdfFile);
        }
    }
}