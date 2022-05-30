namespace Palavyr.Core.Exceptions
{
    public class MultiMessageDomainException : DomainException
    {
        public string[] AdditionalMessages { get; set; }

        public MultiMessageDomainException(string primaryMessage, string[]? additionalMessages = null) : base(primaryMessage)
        {
            AdditionalMessages = additionalMessages ?? new string[] { };
        }
    }
}