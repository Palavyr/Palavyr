namespace Palavyr.Core.Resources
{
    public class CredentialsResource
    {
        public string ApiKey { get; set; }
        public string SessionId { get; set; } // used to access the correct database
        public bool Authenticated { get; set; }
        public string Message { get; set; }
        public string JwtToken { get; set; }
        public string EmailAddress { get; set; }

        public CredentialsResource()
        {
        }

        private CredentialsResource(string sessionId, string apiKey, string jwtToken, string emailAddress, bool authenticated, string message)
        {
            ApiKey = apiKey;
            SessionId = sessionId;
            Authenticated = authenticated;
            Message = message;
            JwtToken = jwtToken;
            EmailAddress = emailAddress;
        }
        
        public static CredentialsResource CreateAuthenticatedResponse(string sessionId, string apiKey, string jwtToken, string emailAddress)
        {
            return new CredentialsResource(sessionId, apiKey, jwtToken, emailAddress, true, "Authentication Successful");
        }

        public static CredentialsResource CreateUnauthenticatedResponse(string message)
        {
            return new CredentialsResource("", "", "", "",false, message);
        }
    }
}