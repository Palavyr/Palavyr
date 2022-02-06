import React, { useContext } from "react";
import { DynamicTable, QuantUnitDefinition, SetState, TableNameMap } from "@Palavyr-Types";
import { Box } from "@material-ui/core";
import { PricingStrategyAuto, PricingStrategySelector } from "./DynamicTableSelector";
import { cloneDeep } from "lodash";
import { useState } from "react";
import { useEffect } from "react";
import { ChangeEvent } from "react";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { TextInput } from "@common/components/TextField/TextInput";
import { useStyles, includesUnit } from "./PricingStrategyTable";

export interface DynamicTableHeaderProps {
    availableDynamicTableOptions: Array<string>;
    tableNameMap: TableNameMap;
    unitTypes: QuantUnitDefinition[];
    inUse: boolean;
    tableTag: string;
    setLocalTable: SetState<DynamicTable>;
    setTables: SetState<DynamicTable[]>;
    setTableTag: SetState<string>;
    localTable: DynamicTable;
}

export const DynamicTableHeader = ({ availableDynamicTableOptions, tableNameMap, unitTypes, inUse, setLocalTable, tableTag, setTableTag, localTable }: DynamicTableHeaderProps) => {
    const cls = useStyles();
    const [disabledSelector, setDisabledSelector] = useState<boolean>(false);
    const { repository } = useContext(DashboardContext);

    useEffect(() => {
        setDisabledSelector(inUse);
    }, []);

    const onPricingStrategyChange = async (event: ChangeEvent<{ name?: string | undefined; value: unknown }>, value: string) => {
        const newTableTypeSelection = value; //event.target.value as string; // will be tableType as shown in the list (Select One Flat)

        // this needs to map to the form used in the table dataresponse format (e.g. SelectOneFlat)
        const newTableTypeSelectionFormatted = tableNameMap[newTableTypeSelection];

        localTable.tableMeta.tableType = newTableTypeSelectionFormatted;
        localTable.tableMeta.prettyName = newTableTypeSelection;

        const updatedTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(localTable.tableMeta);

        localTable.tableMeta = updatedTableMeta;
        setLocalTable(cloneDeep(localTable));
    };

    const onUnitSelect = (event: ChangeEvent<{ name?: string | undefined; value: unknown }>, value: QuantUnitDefinition) => {
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
                    <PricingStrategyAuto
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
                    value={tableTag}
                    onChange={e => {
                        e.preventDefault();
                        setTableTag(e.target.value);
                    }}
                />
            </Box>
        </>
    );
};
