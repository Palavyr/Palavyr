namespace Palavyr.Core.Models.Contracts
{
    public interface IOrderableThreshold
    {
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public double Threshold { get; set; }
        public bool Range { get; set; }
        public bool TriggerFallback { get; set; }
    }
}