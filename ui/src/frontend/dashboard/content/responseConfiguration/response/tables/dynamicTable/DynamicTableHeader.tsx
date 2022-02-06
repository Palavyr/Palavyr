import React, { useContext } from "react";
import { DynamicTable, QuantUnitDefinition, SetState, TableNameMap } from "@Palavyr-Types";
import { Box } from "@material-ui/core";
import { UnitSelector, PricingStrategySelector } from "./DynamicTableSelector";
import { cloneDeep } from "lodash";
import { useState } from "react";
import { useEffect } from "react";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { TextInput } from "@common/components/TextField/TextInput";
import { useStyles, includesUnit } from "./PricingStrategyTable";

export interface DynamicTableHeaderProps {
    availableDynamicTableOptions: Array<string>;
    tableNameMap: TableNameMap;
    unitTypes: QuantUnitDefinition[];
    inUse: boolean;
    setLocalTable: SetState<DynamicTable>;
    setTables: SetState<DynamicTable[]>;
    localTable: DynamicTable;
}

const unpackUnitMeta = (unitId: number, unitTypes: QuantUnitDefinition[]) => {
    const quantDef = unitTypes.find(unit => unit.unitId === unitId);
    if (!quantDef) throw new Error("Quant Def not found - Something is out of alignment....");
    return quantDef;
};

export const DynamicTableHeader = ({ availableDynamicTableOptions, tableNameMap, unitTypes, inUse, setLocalTable, localTable }: DynamicTableHeaderProps) => {
    const cls = useStyles();
    const [disabledSelector, setDisabledSelector] = useState<boolean>(false);
    const { repository } = useContext(DashboardContext);

    useEffect(() => {
        setDisabledSelector(inUse);
    }, [localTable, unitTypes]);

    const onPricingStrategyChange = async (_: any, value: string) => {
        const newTableTypeSelection = value;

        // this needs to map to the form used in the table dataresponse format (e.g. SelectOneFlat)
        const newTableTypeSelectionFormatted = tableNameMap[newTableTypeSelection];

        localTable.tableMeta.tableType = newTableTypeSelectionFormatted;
        localTable.tableMeta.prettyName = newTableTypeSelection;

        const updatedTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(localTable.tableMeta);

        localTable.tableMeta = updatedTableMeta;
        const quantDef = unpackUnitMeta(updatedTableMeta.unitId, unitTypes);

        localTable.tableMeta.unitId = updatedTableMeta.unitId;
        localTable.tableMeta.unitGroup = quantDef.unitGroup;
        localTable.tableMeta.unitPrettyName = quantDef.unitPrettyName;

        const { tableRows } = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(updatedTableMeta.areaIdentifier, updatedTableMeta.tableType, updatedTableMeta.tableId);

        localTable.tableRows = tableRows;
        setLocalTable(cloneDeep(localTable));
    };

    const onUnitSelect = (_: any, value: QuantUnitDefinition) => {
        localTable.tableMeta.unitId = value.unitId;
        localTable.tableMeta.unitGroup = value.unitGroup;
        localTable.tableMeta.unitPrettyName = value.unitPrettyName;
        setLocalTable(cloneDeep(localTable));
    };

    return (
        <>
            <Box className={cls.table}>
                <PricingStrategySelector
                    toolTipTitle={disabledSelector ? "Disabled when pricing strategy is used in the Palavyr configuration." : ""}
                    disabled={disabledSelector}
                    pricingStrategySelection={localTable.tableMeta.prettyName}
                    getOptionLabel={(option: string) => option}
                    handleChange={onPricingStrategyChange}
                    tableOptions={availableDynamicTableOptions}
                    helperText="Select Pricing Strategy"
                />
                {includesUnit(localTable) && (
                    <UnitSelector
                        options={unitTypes.sort((a, b) => -b.unitGroup.localeCompare(a.unitGroup))}
                        groupBy={option => {
                            const upper = option.unitGroup.toUpperCase();
                            return upper.charAt(0) + option.unitGroup.slice(1, upper.length);
                        }}
                        getOptionLabel={def => def.unitPrettyName}
                        toolTipTitle="Unit Selector"
                        disabled={false}
                        selection={unitTypes.filter(x => x.unitPrettyName === localTable.tableMeta.unitPrettyName)[0]}
                        handleChange={onUnitSelect}
                        helperText="Select the unit type for the threshold"
                    />
                )}
                <TextInput
                    className={cls.textinput}
                    label="Short table description (2 or 3 words)"
                    value={localTable.tableMeta.tableTag}
                    onChange={e => {
                        e.preventDefault();
                        localTable.tableMeta.tableTag = e.target.value;
                        setLocalTable(cloneDeep(localTable));
                    }}
                />
            </Box>
        </>
    );
};
