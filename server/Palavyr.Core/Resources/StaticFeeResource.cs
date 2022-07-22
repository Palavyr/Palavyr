namespace Palavyr.Core.Resources
{
    public class StaticFeeResource : EntityResource
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public string FeeId { get; set; }
        public string IntentId { get; set; }
    }
}