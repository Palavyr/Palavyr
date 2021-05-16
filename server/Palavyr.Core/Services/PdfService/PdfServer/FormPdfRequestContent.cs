// using System.Collections.Generic;
// using System.Net.Http;
// using Palavyr.Core.Services.PdfService.PdfServer;
//
// namespace Palavyr.Core.Services.PdfService
// {
//     public class HtmlToPdfRequestContent : Dictionary<string, string>
//     {
//     }
//     
//     public interface IFormPdfRequestContent
//     {
//         FormUrlEncodedContent FormUrlEncodedContent(string htmlString, string identifier, string bucket, string s3Key);
//     }
//
//     public class FormPdfRequestContent : IFormPdfRequestContent
//     {
//         public FormUrlEncodedContent FormUrlEncodedContent(string htmlString, string identifier, string bucket, string s3Key)
//         {
//             var values = new HtmlToPdfRequestContent
//             {
//                 {PdfServerConstants.Bucket, bucket},
//                 {PdfServerConstants.Html, htmlString.Trim()},
//                 {PdfServerConstants.Key, s3Key},
//                 {PdfServerConstants.Id, identifier}, // used as label inside the pdf, not part of the save path
//                 {PdfServerConstants.PaperOrientation, PdfPaperDefaults.PaperOrientation},
//                 {PdfServerConstants.PaperFormat, PdfPaperDefaults.PaperFormat},
//                 {PdfServerConstants.PaperBorder, PdfPaperDefaults.PaperBorder},
//                 {PdfServerConstants.PaperFooterHeight, PdfPaperDefaults.PaperFooterHeight},
//                 {PdfServerConstants.PaperFooterContentsDefault, PdfPaperDefaults.PaperFooterContentsDefault(identifier)}
//             };
//
//             var content = new FormUrlEncodedContent(values);
//             return content;
//         }
//     }
// }