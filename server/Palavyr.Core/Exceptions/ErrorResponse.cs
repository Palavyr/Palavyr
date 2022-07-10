using Newtonsoft.Json;

namespace Palavyr.Core.Exceptions
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string[] AdditionalMessages { get; set; }
        public int StatusCode { get; set; }


        public ErrorResponse()
        {
        }

        public ErrorResponse(string messages, string[] additionalMessages, int statusCode)
        {
            Message = messages;
            AdditionalMessages = additionalMessages;
            StatusCode = statusCode;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}