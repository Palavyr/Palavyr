namespace Palavyr.Core.Models.Resources.Responses
{
    public class SendEmailResultResponse
    {
        public string NextNodeId { get; set; }
        public bool Result { get; set; }
        
        public static SendEmailResultResponse CreateSuccess(string nextNodeId)
        {
            return new SendEmailResultResponse()
            {
                NextNodeId = nextNodeId,
                Result = true
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