﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Palavyr.Domain.Contracts;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.Domain.Configuration.Schemas
{
    public class SelectOneFlat : IOrderedTable, IDynamicTable<SelectOneFlat>
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
        public int RowOrder { get; set; }

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

        public SelectOneFlat CreateTemplate(string accountId, string areaIdentifier, string tableId)
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

        public List<SelectOneFlat> UpdateTable(DynamicTable table)
        {
            var mappedTableRows = new List<SelectOneFlat>();
            foreach (var row in table.SelectOneFlat)
            {
                var mappedRow = CreateNew(
                    row.AccountId,
                    row.AreaIdentifier,
                    row.Option,
                    row.ValueMin,
                    row.ValueMax,
                    row.Range,
                    row.TableId
                );
                mappedTableRows.Add(mappedRow);
            }

            return mappedTableRows;
        }
    }
}