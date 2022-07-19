
using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Data.Entities.PricingStrategyTables
{
    public class SimpleThresholdTableRow : Entity, IOrderedTable, IPricingStrategyTable<SimpleThresholdTableRow>, IHaveRange, IOrderableThreshold, IHaveAccountId
    {
        private const string PrettyName = "Simple Threshold";

        public SimpleThresholdTableRow()
        {
        }

        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public string TableId { get; set; }
        public string RowId { get; set; }
        public double Threshold { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public string ItemName { get; set; }
        public int RowOrder { get; set; }
        public bool TriggerFallback { get; set; }

        public SimpleThresholdTableRow CreateNew(
            string accountId,
            string intentId,
            string tableId,
            string itemName,
            string rowId,
            double threshold,
            double valueMin,
            double valueMax,
            bool range,
            bool triggerFallback,
            int rowOrder)
        {
            return new SimpleThresholdTableRow()
            {
                AccountId = accountId,
                IntentId = intentId,
                TableId = tableId,
                ItemName = itemName,
                RowId = rowId,
                Threshold = threshold,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range,
                TriggerFallback = triggerFallback,
                RowOrder = rowOrder
            };
        }

        public SimpleThresholdTableRow CreateTemplate(string accountId, string intentId, string tableId)
        {
            return new SimpleThresholdTableRow()
            {
                AccountId = accountId,
                IntentId = intentId,
                TableId = tableId,
                RowId = StaticGuidUtils.CreateNewId()
            };
        }

        public List<SimpleThresholdTableRow> UpdateTable(PricingStrategyTable<SimpleThresholdTableRow> table)
        {
            var mappedTableRows = new List<SimpleThresholdTableRow>();
            foreach (var row in table.TableData)
            {
                var mappedRow = CreateNew(
                    row.AccountId,
                    row.IntentId,
                    row.TableId,
                    row.ItemName,
                    row.RowId,
                    row.Threshold,
                    row.ValueMin,
                    row.ValueMax,
                    row.Range,
                    row.TriggerFallback,
                    row.RowOrder
                );
                mappedTableRows.Add(mappedRow);
            }
        
            return mappedTableRows;
        }

        public bool EnsureValid()
        {
            return true;
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