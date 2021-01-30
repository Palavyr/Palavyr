namespace EmailService.Verification
{
    public class EmailVerificationResponse
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }

        public const string Pending = "Pending";
        public const string Success = "Success";
        public const string Failed = "Failed";

        public static EmailVerificationResponse CreateNew(string status, string message, string title)
        {
            return new EmailVerificationResponse()
            {
                Title = title,
                Message = message,
                Status = status
            };
        }

        public static EmailVerificationResponse CreatePending(string message, string title)
        {
            return new EmailVerificationResponse()
            {
                Title = title,
                Message = message,
                Status = Pending
            };
        }

        public static EmailVerificationResponse CreateFailed(string message, string title)
        {
            return new EmailVerificationResponse()
            {
                Title = title,
                Message = message,
                Status = Failed
            };
        }

        public static EmailVerificationResponse CreateIsVerified(string message, string title)
        {
            return new EmailVerificationResponse()
            {
                Title = title,
                Message = message,
                Status = Success
            };
        }

        public bool IsFailed() => Status == Failed;
        public bool IsPending() => Status == Pending;
        public bool IsVerified() => Status == Success;
    }
}