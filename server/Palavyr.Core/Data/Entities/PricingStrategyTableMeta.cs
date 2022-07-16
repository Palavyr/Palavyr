#nullable disable

using System;
using System.Collections.Generic;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Contracts;
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
        public string AreaIdentifier { get; set; }
        public bool ValuesAsPaths { get; set; } = false; // for tables that specify various options, whether or not to use each option to create a new tree path.
        public bool UseTableTagAsResponseDescription { get; set; } = false;
        public UnitIds UnitId { get; set; }

        public static PricingStrategyTableMeta CreateNew(
            string tableTag,
            string prettyName,
            string tableType,
            string tableId,
            string areaId,
            string accountId,
            UnitIds unitId
        )
        {
            return new PricingStrategyTableMeta
            {
                TableId = tableId,
                TableType = tableType,
                TableTag = tableTag,
                AreaIdentifier = areaId,
                AccountId = accountId,
                PrettyName = prettyName,
                UnitId = unitId,
                
            };
        }

        public static List<PricingStrategyTableMeta> CreateDefaultMetas(string areaId, string accountId)
        {
            return new List<PricingStrategyTableMeta>
            {
                CreateNew(
                    "default",
                    new SimpleSelectTableRow().GetPrettyName(), // TODO: Do this better
                    new SimpleSelectTableRow().GetTableType(),
                    Guid.NewGuid().ToString(),
                    areaId,
                    accountId,
                    UnitIds.Currency)
            };
        }

        public void Deconstruct(out string areaId, out string tableId)
        {
            areaId = AreaIdentifier;
            tableId = TableId;
        }

        public void UpdateProperties(ModifyDynamicTableMetaRequest metaUpdate, IUnitRetriever unitRetriever)
        {
            if (string.IsNullOrEmpty(metaUpdate.TableType) || string.IsNullOrWhiteSpace(metaUpdate.TableType)) throw new DomainException("Table Type is a required field");

            if (string.IsNullOrEmpty(metaUpdate.PrettyName) || string.IsNullOrWhiteSpace(metaUpdate.PrettyName)) throw new DomainException("Table Pretty Name is a required field");

            TableTag = metaUpdate.TableTag;
            TableType = metaUpdate.TableType;
            ValuesAsPaths = metaUpdate.ValueAsPaths;
            PrettyName = metaUpdate.PrettyName;
            UnitId = unitRetriever.ConvertToUnitId(metaUpdate.UnitId.ToString());
        }
    }
}