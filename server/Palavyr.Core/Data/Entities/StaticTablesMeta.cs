﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Data.Entities
{
    public class StaticTablesMeta : Entity, IStaticTable, IHaveAccountId
    {
        public int TableOrder { get; set; }
        public string Description { get; set; }
        public string IntentId { get; set; }
        public List<StaticTableRow> StaticTableRows { get; set; } = new List<StaticTableRow>();
        public string AccountId { get; set; }
        public bool PerPersonInputRequired { get; set; }
        public bool IncludeTotals { get; set; }
        public string TableId { get; set; }


        [NotMapped]
        private static string DefaultDescription { get; } = "Default Description";

        public static StaticTablesMeta CreateNewMetaTemplate(string intentId, string accountId)
        {
            return new StaticTablesMeta
            {
                TableOrder = 0,
                Description = DefaultDescription,
                IntentId = intentId,
                StaticTableRows = StaticTableRow.CreateDefaultStaticTable(0, intentId, accountId),
                PerPersonInputRequired = false,
                IncludeTotals = true,
                TableId = Guid.NewGuid().ToString()
            };
        }

        public static List<StaticTablesMeta> BindTemplateList(List<StaticTablesMeta> staticTablesMetas, string accountId)
        {
            var boundMetas = new List<StaticTablesMeta>() { };
            boundMetas.AddRange(
                staticTablesMetas.Select(
                    meta => new StaticTablesMeta()
                    {
                        TableOrder = meta.TableOrder,
                        Description = meta.Description,
                        IntentId = meta.IntentId,
                        AccountId = accountId,
                        StaticTableRows = StaticTableRow.BindTemplateList(meta.StaticTableRows, accountId),
                        PerPersonInputRequired = meta.PerPersonInputRequired,
                        IncludeTotals = meta.IncludeTotals
                    }));
            return boundMetas;
        }
    }
}