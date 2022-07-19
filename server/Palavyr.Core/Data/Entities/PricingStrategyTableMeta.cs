using System;
using System.Collections.Generic;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.Units;

namespace Palavyr.Core.Data.Entities
{
    public class PricingStrategyTableMeta : Entity, ITable, IHaveAccountId
    {
        public string TableTag { get; set; }
        public string PrettyName { get; set; }
        public string TableType { get; set; }
        public string TableId { get; set; }
        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public bool ValuesAsPaths { get; set; } = false; // for tables that specify various options, whether or not to use each option to create a new tree path.
        public bool UseTableTagAsResponseDescription { get; set; } = false;
        public UnitIds UnitId { get; set; }

        public static PricingStrategyTableMeta CreateNew(
            string tableTag,
            string prettyName,
            string tableType,
            string tableId,
            string intentId,
            string accountId,
            UnitIds unitId
        )
        {
            return new PricingStrategyTableMeta
            {
                TableId = tableId,
                TableType = tableType,
                TableTag = tableTag,
                IntentId = intentId,
                AccountId = accountId,
                PrettyName = prettyName,
                UnitId = unitId,
                ValuesAsPaths = false, // for tables that specify various options, whether or not to use each option to create a new tree path.
                UseTableTagAsResponseDescription = false,
            };
        }

        public static List<PricingStrategyTableMeta> CreateDefaultMetas(string intentId, string accountId)
        {
            return new List<PricingStrategyTableMeta>
            {
                CreateNew(
                    "default",
                    new CategorySelectTableRow().GetPrettyName(), // TODO: Do this better
                    new CategorySelectTableRow().GetTableType(),
                    Guid.NewGuid().ToString(),
                    intentId,
                    accountId,
                    UnitIds.Currency)
            };
        }

        public void Deconstruct(out string intentId, out string tableId)
        {
            intentId = IntentId;
            tableId = TableId;
        }

        public void UpdateProperties(PricingStrategyTableMetaResource metaUpdate, IUnitRetriever unitRetriever)
        {
            if (string.IsNullOrEmpty(metaUpdate.TableType) || string.IsNullOrWhiteSpace(metaUpdate.TableType)) throw new DomainException("Table Type is a required field");

            if (string.IsNullOrEmpty(metaUpdate.PrettyName) || string.IsNullOrWhiteSpace(metaUpdate.PrettyName)) throw new DomainException("Table Pretty Name is a required field");

            TableTag = metaUpdate.TableTag;
            TableType = metaUpdate.TableType;
            ValuesAsPaths = metaUpdate.ValuesAsPaths;
            PrettyName = metaUpdate.PrettyName;
            UnitId = unitRetriever.ConvertToUnitId(metaUpdate.UnitId.ToString());
        }
    }
}