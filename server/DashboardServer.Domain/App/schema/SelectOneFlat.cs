using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Domain.DynamicTables
{
    public class SelectOneFlat
    {
        [Key] 
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string AreaId { get; set; }
        public string TableId { get; set; }
        public string Option { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public string TableTag { get; set; }

        public static SelectOneFlat CreateNew(string accountId, string areaId, string option, double valueMin, double valueMax, bool range, string tableId, string tableTag)
        {
            return new SelectOneFlat()
            {
                AccountId = accountId,
                AreaId = areaId,
                TableId = tableId,
                TableTag = tableTag,
                Option = option,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range
            };
        }

        public static SelectOneFlat CreateTemplate(string accountId, string areaId, string tableId)
        {
            return new SelectOneFlat()
            {
                AccountId = accountId,
                AreaId = areaId,
                Option = "Option Placeholder",
                ValueMin = 0.00,
                ValueMax = 0.00,
                Range = true,
                TableId = tableId,
                TableTag = "Default Tag"
            };
        }
    }
}