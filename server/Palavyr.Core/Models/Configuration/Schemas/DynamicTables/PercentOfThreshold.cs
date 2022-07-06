#nullable disable

using System;
using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Models.Configuration.Schemas.DynamicTables
{
    /// <summary>
    /// The table meta provides a tableID, for this dynamic table though, we allow the records to be split into subtables
    /// so that we can associate multiple different items with the same requested value.
    /// So 1 value for a house might provide 3 different items, each with a different threshold by which pricing is determined.
    /// The ItemId/ItemName represents this partition key.
    /// The itemName unfortunately has to be duplicated along with the itemId.
    /// </summary>
    public class PercentOfThreshold : Entity, IComparable<PercentOfThreshold>, IOrderedTable, IPricingStrategyTable<PercentOfThreshold>, IOrderableThreshold, IMultiItem, IHaveAccountId
    {
        private const string PrettyName = "Percent Of Threshold";


        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public string RowId { get; set; }
        public double Threshold { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public double Modifier { get; set; }
        public bool PosNeg { get; set; }
        public int RowOrder { get; set; }
        public bool TriggerFallback { get; set; }

        public int ItemOrder { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; } // unfortunate - doesn't fit in meta, and here it will be duplicated - we don't keep a table for the subtables held by this

        public static PercentOfThreshold CreateNew(
            string accountId,
            string areaIdentifier,
            string tableId,
            string rowId,
            double threshold,
            double modifier,
            string itemName,
            string itemId,
            double valueMin,
            double valueMax,
            bool range,
            bool posNeg,
            bool triggerFallback
        )
        {
            return new PercentOfThreshold()
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                RowId = rowId,
                Threshold = threshold,
                Modifier = modifier,
                ItemName = itemName,
                ItemId = itemId,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range,
                PosNeg = posNeg,
                TriggerFallback = triggerFallback
            };
        }

        public PercentOfThreshold CreateTemplate(string accountId, string areaIdentifier, string tableId)
        {
            return new PercentOfThreshold()
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                RowId = StaticGuidUtils.CreateNewId(),
                Threshold = 0.00,
                Modifier = 0.00,
                ItemName = "Default Item",
                ItemId = StaticGuidUtils.CreateNewId(),
                ValueMin = 0.00,
                ValueMax = 0.00,
                Range = false,
                PosNeg = true
            };
        }

        public List<PercentOfThreshold> UpdateTable(PricingStrategyTable<PercentOfThreshold> table)
        {
            var mappedTableRows = new List<PercentOfThreshold>();
            foreach (var row in table.TableData)
            {
                var mappedRow = CreateNew(
                    row.AccountId,
                    row.AreaIdentifier,
                    row.TableId,
                    row.RowId,
                    row.Threshold,
                    row.Modifier,
                    row.ItemName,
                    row.ItemId,
                    row.ValueMin,
                    row.ValueMax,
                    row.Range,
                    row.PosNeg,
                    row.TriggerFallback
                );
                mappedTableRows.Add(mappedRow);
            }

            return mappedTableRows;
        }

        public bool EnsureValid()
        {
            return true;
        }

        public int CompareTo(PercentOfThreshold other)
        {
            return other.Threshold.CompareTo(Threshold);
        }

        public string GetPrettyName()
        {
            return PrettyName;
        }


        public string GetTableType()
        {
            return GetType().Name;
        }
    }
}