namespace Palavyr.Core.Models.Contracts
{
    public interface IHaveRange
    {
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
    }
}