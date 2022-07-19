import React, { useContext } from "react";
import { PricingStrategy, PricingStrategyTableTypeResource, QuantUnitDefinition, SetState, TableNameMap } from "@Palavyr-Types";
import { Box } from "@material-ui/core";
import { UnitSelector, PricingStrategySelector } from "./PricingStrategySelector";
import { cloneDeep } from "lodash";
import { useState } from "react";
import { useEffect } from "react";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { TextInput } from "@common/components/TextField/TextInput";
import { useStyles, includesUnit } from "./PricingStrategyTable";

export interface PricingStrategyHeaderProps {
    availablePricingStrategyOptions: PricingStrategyTableTypeResource[];
    unitTypes: QuantUnitDefinition[];
    inUse: boolean;
    setLocalTable: SetState<PricingStrategy>;
    setTables: SetState<PricingStrategy[]>;
    localTable: PricingStrategy;
}

const unpackUnitMeta = (unitId: number, unitTypes: QuantUnitDefinition[]) => {
    const quantDef = unitTypes.find(unit => unit.unitId === unitId);
    if (!quantDef) throw new Error("Quant Def not found - Something is out of alignment....");
    return quantDef;
};

export const PricingStrategyHeader = ({ availablePricingStrategyOptions, unitTypes, inUse, setLocalTable, localTable }: PricingStrategyHeaderProps) => {
    const cls = useStyles();
    const [disabledSelector, setDisabledSelector] = useState<boolean>(false);
    const { repository } = useContext(DashboardContext);

    useEffect(() => {
        setDisabledSelector(inUse);
        return () => {
            setDisabledSelector(false);
        };
    }, [localTable, unitTypes]);

    const onPricingStrategyChange = async (_: any, value: PricingStrategyTableTypeResource) => {
        // this needs to map to the form used in the table dataresponse format (e.g. SelectOneFlat)

        localTable.tableMeta.tableType = value.tableType;
        localTable.tableMeta.prettyName = value.prettyName;

        const updatedTableMeta = await repository.Configuration.Tables.Dynamic.ModifyPricingStrategyMeta(localTable.tableMeta);

        localTable.tableMeta = updatedTableMeta;
        const quantDef = unpackUnitMeta(updatedTableMeta.unitId, unitTypes);

        localTable.tableMeta.unitId = updatedTableMeta.unitId;
        localTable.tableMeta.unitGroup = quantDef.unitGroup;
        localTable.tableMeta.unitPrettyName = quantDef.unitPrettyName;

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
                    pricingStrategySelection={availablePricingStrategyOptions.filter((x: PricingStrategyTableTypeResource) => x.tableType === localTable.tableMeta.tableType)[0]}
                    getOptionLabel={(option: PricingStrategyTableTypeResource) => option.prettyName}
                    handleChange={onPricingStrategyChange}
                    tableOptions={availablePricingStrategyOptions}
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
                    label="Table Name (used in the chat configuration)"
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
