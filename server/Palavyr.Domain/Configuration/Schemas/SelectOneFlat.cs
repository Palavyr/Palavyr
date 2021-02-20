using System.ComponentModel.DataAnnotations;

namespace Palavyr.Domain.Configuration.Schemas
{
    public class SelectOneFlat
    {
        [Key] 
        public int? Id { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public string Option { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }

        public static SelectOneFlat CreateNew(string accountId, string areaIdentifier, string option, double valueMin, double valueMax, bool range, string tableId)
        {
            return new SelectOneFlat()
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                Option = option,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range
            };
        }

        public static SelectOneFlat CreateTemplate(string accountId, string areaIdentifier, string tableId)
        {
            return new SelectOneFlat()
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                Option = "Option Placeholder",
                ValueMin = 0.00,
                ValueMax = 0.00,
                Range = true,
                TableId = tableId,
            };
        }
    }
}