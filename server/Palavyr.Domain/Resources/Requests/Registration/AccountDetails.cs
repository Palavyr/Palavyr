namespace Palavyr.Domain.Resources.Requests.Registration
{
    public class AccountDetails
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string EmailAddress { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string LogoUri { get; set; }
        public string Locale { get; set; }


        public static AccountDetails CreateViaGoogleLogin(string emailAddress)
        {
            return new AccountDetails()
            {
                EmailAddress = emailAddress,
                Password = null
            };
        }
    }
}