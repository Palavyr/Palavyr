namespace Palavyr.API.response
{
    public class EmailVerificationResponse
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }

        public static EmailVerificationResponse CreateNew(string status, string message, string title)
        {
            return new EmailVerificationResponse()
            {
                Title = title,
                Message = message,
                Status = status
            };
        }
    }
}