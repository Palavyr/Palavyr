namespace Palavyr.API.ResponseTypes
{
    public class Credentials
    {
        public string ApiKey { get; set; }
        public string SessionId { get; set; } // used to access the correct database
        public bool Authenticated { get; set; }
        public string Message { get; set; }
        public string JwtToken { get; set; }
        public string EmailAddress { get; set; }


        Credentials(string sessionId, string apiKey, string jwtToken, string emailAddress, bool authenticated, string message)
        {
            ApiKey = apiKey;
            SessionId = sessionId;
            Authenticated = authenticated;
            Message = message;
            JwtToken = jwtToken;
            EmailAddress = emailAddress;
        }


        public static Credentials CreateAuthenticatedResponse(string sessionId, string apiKey, string jwtToken, string emailAddress)
        {
            return new Credentials(sessionId, apiKey, jwtToken, emailAddress, true, "Authentication Successful");
        }

        public static Credentials CreateUnauthenticatedResponse(string message)
        {
            return new Credentials("", "", "", "",false, message);
        }
    }
}