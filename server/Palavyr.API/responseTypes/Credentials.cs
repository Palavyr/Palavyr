namespace Palavyr.API.ResponseTypes
{
    public class Credentials
    {
        public string ApiKey { get; set; }
        public string SessionId { get; set; } // used to access the correct database
        public bool Authenticated { get; set; }
        public string Message { get; set; }


        Credentials(string sessionId, string apiKey, bool authenticated, string message)
        {
            ApiKey = apiKey;
            SessionId = sessionId;
            Authenticated = authenticated;
            Message = message;
        }

        public static Credentials CreateAuthenticatedResponse(string sessionId, string apiKey)
        {
            return new Credentials(sessionId, apiKey, true, "Authentication Successful");
        }

        public static Credentials CreateUnauthenticatedResponse(string message)
        {
            return new Credentials("", "", false, message);
        }
    }
}