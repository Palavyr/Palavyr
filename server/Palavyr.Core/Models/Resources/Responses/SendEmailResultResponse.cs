#nullable enable
namespace Palavyr.Core.Models.Resources.Responses
{
    public class SendEmailResultResponse
    {
        public string NextNodeId { get; set; }
        public bool Result { get; set; }
        public string? PdfLink { get; set; }
        
        public static SendEmailResultResponse CreateSuccess(string nextNodeId, string? pdfLink)
        {
            return new SendEmailResultResponse()
            {
                NextNodeId = nextNodeId,
                Result = true,
                PdfLink = pdfLink
            };
        }

        public static SendEmailResultResponse CreateFailure(string nextNodeId)
        {
            return new SendEmailResultResponse
            {
                NextNodeId = nextNodeId,
                Result = false
            };
        }
    }
}