﻿namespace Palavyr.API.ReceiverTypes
{
    public class LoginCredentials
    {
        // Default
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string sessionToken { get; set; }
        
        public string OldPassword { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        
        //Google
        public string OneTimeCode { get; set; }
        public string TokenId { get; set; }
    }

    public class StatusCredentials
    {
        public string JwtToken { get; set; }
        public string SessionId { get; set; }
    }

    public class LogoutCredentials
    {
        public string SessionId { get; set; }
    }
    
    public class GoogleRegistrationDetails
    {
        public string OneTimeCode { get; set; }
        public string AccessToken { get; set; }
        public string TokenId { get; set; }
    }
    
}