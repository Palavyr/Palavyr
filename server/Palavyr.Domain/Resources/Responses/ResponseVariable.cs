namespace Palavyr.Domain.Resources.Responses
{
    public class ResponseVariable
    {
        public string Name { get; set; }
        public string Pattern { get; set; }
        public string Details { get; set; }

        public ResponseVariable(string name, string pattern, string details)
        {
            Name = name;
            Details = details;
            Pattern = pattern;
        }
    }
}