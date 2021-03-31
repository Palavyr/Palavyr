namespace Palavyr.Core.Models.Resources.Responses
{
    public class AccountEmailSettingsResponse
    {
        public string EmailAddress { get; set; }
        public bool IsVerified { get; set; }
        public bool AwaitingVerification { get; set; }

        public static AccountEmailSettingsResponse CreateNew(
            string emailAddress, 
            bool isVerified,
            bool awaitingVerification)
        {
            return new AccountEmailSettingsResponse()
            {
                EmailAddress = emailAddress,
                IsVerified = isVerified,
                AwaitingVerification = awaitingVerification,
            };
        }
    }
    
}