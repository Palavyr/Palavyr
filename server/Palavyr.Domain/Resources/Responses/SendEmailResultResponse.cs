namespace Palavyr.Domain.Resources.Responses
{
    public class SendEmailResultResponse
    {
        public string NextNodeId { get; set; }
        public bool Result { get; set; }
        
        public static SendEmailResultResponse Create(string nextNodeId, bool result)
        {
            return new SendEmailResultResponse()
            {
                NextNodeId = nextNodeId,
                Result = result
            };
        }
    }
}