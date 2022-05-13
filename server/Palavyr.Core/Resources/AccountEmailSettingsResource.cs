namespace Palavyr.Core.Resources.Responses
{
    public class AccountEmailSettingsResource
    {
        public string EmailAddress { get; set; }
        public bool IsVerified { get; set; }
        public bool AwaitingVerification { get; set; }

        public static AccountEmailSettingsResource CreateNew(
            string emailAddress, 
            bool isVerified,
            bool awaitingVerification)
        {
            return new AccountEmailSettingsResource()
            {
                EmailAddress = emailAddress,
                IsVerified = isVerified,
                AwaitingVerification = awaitingVerification,
            };
        }
    }
    
}