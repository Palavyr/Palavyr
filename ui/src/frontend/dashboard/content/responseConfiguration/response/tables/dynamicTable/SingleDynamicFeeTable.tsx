import React, { useContext } from "react";
import { DynamicTableMeta, DynamicTableMetas, DynamicTableProps, QuantUnitDefinition, TableData, TableNameMap } from "@Palavyr-Types";
import { makeStyles, Box } from "@material-ui/core";
import { PricingStrategyAuto, PricingStrategySelector } from "./DynamicTableSelector";
import { removeByIndex } from "@common/utils";
import { cloneDeep } from "lodash";
import { useState } from "react";
import { useCallback } from "react";
import { useEffect } from "react";
import { ChangeEvent } from "react";
import { dynamicTableComponentMap } from "./DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { TextInput } from "@common/components/TextField/TextInput";

const useStyles = makeStyles(theme => ({
    headerCol: {
        textAlign: "center",
        fontSize: "16pt",
    },
    centerText: {
        textAlign: "center",
    },
    header: {
        paddingTop: ".3rem",
        paddingBottom: ".3rem",
        backgroundColor: theme.palette.secondary.light,
    },
    section: {
        border: "none",
        margin: "1.2rem",
        borderRadius: "15px",
        background: theme.palette.secondary.light,
        boxShadow: "none",
    },
    table: {
        border: "none",
        background: theme.palette.secondary.light,
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        borderRadius: "10px",
    },
    textinput: {
        margin: "0.9rem",
        width: "50ch",
    },
}));

const includesUnit = (tableProps: DynamicTableProps) => {
    const allowedDynamicTypes = ["CategoryNestedThreshold", "BasicThreshold", "PercentOfThreshold"]; // Sorry - TODO / Add identifier from server
    return allowedDynamicTypes.includes(tableProps.tableMeta.tableType);
};

export interface SingleDynamicFeeTableProps {
    tableMeta: DynamicTableMeta;
    availablDynamicTableOptions: Array<string>;
    tableNameMap: TableNameMap;
    parentState: boolean;
    changeParentState: any;
    areaIdentifier: string;
    tableMetaIndex: number;
    tableMetas: DynamicTableMetas;
    setTableMetas: any;
    setLoaded: any;
    showDebug: boolean;
    unitTypes: QuantUnitDefinition[];
}

export const SingleDynamicFeeTable = ({
    showDebug,
    setLoaded,
    tableMetaIndex,
    tableMetas,
    setTableMetas,
    tableMeta,
    availablDynamicTableOptions,
    tableNameMap,
    parentState,
    changeParentState,
    areaIdentifier,
    unitTypes,
}: SingleDynamicFeeTableProps) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const [tableTag, setTableTag] = useState<string>("");
    const [disabledSelector, setDisabledSelector] = useState<boolean>(false);

    const [tableProps, setTableProps] = useState<DynamicTableProps | null>(null);

    const setTableRows = (newRows: any[]) => {
        if (tableProps) {
            tableProps.setTableRows(newRows);
        }
        setTableProps(cloneDeep(tableProps));
    };

    const loadDynamicData = useCallback(async () => {
        const { tableRows, isInUse } = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(areaIdentifier, tableMeta.tableType, tableMeta.tableId);

        const rows = tableRows as TableData;

        setTableProps(
            cloneDeep({
                tableId: tableMeta.tableId,
                areaIdentifier: areaIdentifier,
                showDebug: showDebug,
                setTableMeta: setTableProps,
                tableMeta: tableMeta,
                tableTag: tableTag,
                tableRows: rows,
                setTableRows: setTableRows,
                deleteAction: deleteAction,
            })
        );

        setTableTag(tableMeta.tableTag);
        setDisabledSelector(isInUse);
    }, [tableMeta, areaIdentifier]);

    useEffect(() => {
        loadDynamicData();
    }, [loadDynamicData, tableMeta, areaIdentifier]);

    useEffect(() => {
        if (tableProps) {
            setTableProps({
                ...tableProps,
                showDebug,
                tableTag,
            });
        }
    }, [showDebug, tableTag]);

    const onPricingStrategyChange = async (event: ChangeEvent<{ name?: string | undefined; value: unknown }>, value: string) => {
        if (tableProps !== null) {
            const newTableTypeSelection = value; //event.target.value as string; // will be tableType as shown in the list (Select One Flat)
            // this needs to map to the form used in the table dataresponse format (e.g. SelectOneFlat)
            const newTableTypeSelectionFormatted = tableNameMap[newTableTypeSelection];

            tableProps.tableMeta.tableType = newTableTypeSelectionFormatted;
            tableProps.tableMeta.prettyName = newTableTypeSelection;

            const updatedTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(tableProps.tableMeta);

            tableProps.tableMeta = updatedTableMeta;
            setTableProps(cloneDeep(tableProps));
        }
    };

    const onUnitSelect = (event: ChangeEvent<{ name?: string | undefined; value: unknown }>, value: QuantUnitDefinition) => {
        if (tableProps !== null) {
            tableProps.tableMeta.unitId = value.unitId;
            tableProps.tableMeta.unitGroup = value.unitGroup;
            tableProps.tableMeta.unitPrettyName = value.unitPrettyName;
            setTableProps(cloneDeep(tableProps));
        }
    };

    const deleteAction = async () => {
        if (tableProps !== null) {
            await repository.Configuration.Tables.Dynamic.deleteDynamicTable(areaIdentifier, tableProps.tableMeta.tableType, tableProps.tableMeta.tableId);
            const newTableMetas = removeByIndex(tableMetas, tableMetaIndex);
            setTableMetas(cloneDeep(newTableMetas));
            changeParentState(!parentState);
            setLoaded(false);
        }
    };

    const DynamicTableComponent = tableMeta && tableMeta.tableType ? dynamicTableComponentMap[tableMeta.tableType] : null;
    // console.log("DYN " + DynamicTableComponent === null)
    return (
        <>
            {tableProps && tableProps.tableMeta && (
                <section className={cls.section}>
                    <Box className={cls.table}>
                        <PricingStrategySelector
                            toolTipTitle={disabledSelector ? "Disabled when pricing strategy is used in the Palavyr configuration." : ""}
                            disabled={disabledSelector}
                            pricingStrategySelection={tableProps.tableMeta.prettyName}
                            getOptionLabel={(option: string) => option}
                            handleChange={onPricingStrategyChange}
                            tableOptions={availablDynamicTableOptions}
                            helperText="Select Pricing Strategy"
                        />
                        {includesUnit(tableProps) && (
                            <PricingStrategyAuto
                                options={unitTypes.sort((a, b) => -b.unitGroup.localeCompare(a.unitGroup))}
                                groupBy={option => {
                                    const upper = option.unitGroup.toUpperCase();
                                    return upper.charAt(0) + option.unitGroup.slice(1, upper.length);
                                }}
                                getOptionLabel={def => def.unitPrettyName}
                                toolTipTitle="Unit Selector"
                                disabled={false}
                                selection={unitTypes.filter(x => x.unitPrettyName === tableProps.tableMeta.unitPrettyName)[0]}
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
                    {DynamicTableComponent !== null ? <DynamicTableComponent {...tableProps} /> : <div>Loading...</div>}
                </section>
            )}
        </>
    );
};
