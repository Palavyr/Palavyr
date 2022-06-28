
namespace Palavyr.Core.Resources
{
    public class TreeErrorsResource
    {
        public string[] MissingNodes { get; set; }
        public string[] OutOfOrder { get; set; }
        public bool AnyErrors { get; set; }

        public TreeErrorsResource(string[] missingNodes, string[]? outOfOrder)
        {
            MissingNodes = missingNodes;
            OutOfOrder = outOfOrder ?? new string[] { };
            AnyErrors = MissingNodes.Length > 0 || OutOfOrder.Length > 0;
        }
    }
}