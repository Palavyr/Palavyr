namespace Palavyr.Core.Models.Resources.Responses
{
    public class TreeErrorsResponse
    {
        public string[] MissingNodes { get; set; }
        public string[] OutOfOrder { get; set; }
        public bool AnyErrors { get; set; }

        public TreeErrorsResponse(string[] missingNodes, string[] outOfOrder)
        {
            MissingNodes = missingNodes;
            OutOfOrder = outOfOrder;
            AnyErrors = MissingNodes.Length > 0 || OutOfOrder.Length > 0;
        }
    }
}