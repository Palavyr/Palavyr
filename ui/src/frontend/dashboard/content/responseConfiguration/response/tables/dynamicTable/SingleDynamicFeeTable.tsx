import React, { useContext } from "react";
import { DynamicTableMeta, DynamicTableMetas, DynamicTableProps, TableData, TableNameMap } from "@Palavyr-Types";
import { TextField, makeStyles, Typography, Table, TableRow, TableCell, TableBody, Box } from "@material-ui/core";
import { DynamicTableSelector } from "./DynamicTableSelector";
import { removeByIndex } from "@common/utils";
import { cloneDeep } from "lodash";
import { useState } from "react";
import { useCallback } from "react";
import { useEffect } from "react";
import { ChangeEvent } from "react";
import { dynamicTableComponentMap } from "./DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import Fade from "react-reveal/Fade";
import { TextInput } from "@common/components/TextField/TextInput";

export interface SingleDynamicFeeTableProps {
    defaultTableMeta: DynamicTableMeta;
    availablDynamicTableOptions: Array<string>;
    tableNameMap: TableNameMap;
    parentState: boolean;
    changeParentState: any;
    areaIdentifier: string;
    tableMetaIndex: number;
    tableMetas: DynamicTableMetas;
    setTableMetas: any;
    setLoaded: any;
    tableNumber: number;
    showDebug: boolean;
}

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
        // padding: "0.5rem",
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
    selector: {},
}));

export const SingleDynamicFeeTable = ({
    showDebug,
    tableNumber,
    setLoaded,
    tableMetaIndex,
    tableMetas,
    setTableMetas,
    defaultTableMeta,
    availablDynamicTableOptions,
    tableNameMap,
    parentState,
    changeParentState,
    areaIdentifier,
}: SingleDynamicFeeTableProps) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const [tableMeta, setTableMeta] = useState<DynamicTableMeta | undefined>();
    const [dynamicTableData, setDynamicTableData] = useState<TableData>();
    const [selection, setSelection] = useState<string>(""); // just the node type
    const [tableTag, setTableTag] = useState<string>("");
    const [disabledSelector, setDisabledSelector] = useState<boolean>(false);

    const loadDynamicData = useCallback(async () => {
        setTableMeta(defaultTableMeta);
        const { tableRows, isInUse } = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(areaIdentifier, defaultTableMeta.tableType, defaultTableMeta.tableId);

        setDynamicTableData(tableRows);
        setSelection(defaultTableMeta.prettyName);
        setTableTag(defaultTableMeta.tableTag);
        setDisabledSelector(isInUse);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [tableMeta]);

    useEffect(() => {
        loadDynamicData();
    }, [loadDynamicData, tableMetas]);

    useEffect(() => {
        (async () => {
            if (tableMeta !== undefined) {
                const { tableRows, isInUse } = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(areaIdentifier, defaultTableMeta.tableType, defaultTableMeta.tableId);
                setDynamicTableData(tableRows);
            }
        })();
        return () => {};

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, tableMeta, selection]);

    const handleChange = async (event: ChangeEvent<{ name?: string | undefined; value: unknown }>) => {
        const newTableTypeSelection = event.target.value as string; // will be tableType as shown in the list (Select One Flat)
        // this needs to map to the form used in the table dataresponse format (e.g. SelectOneFlat)
        const newTableTypeSelectionFormatted = tableNameMap[newTableTypeSelection];

        if (tableMeta !== undefined) {
            tableMeta.tableType = newTableTypeSelectionFormatted;
            tableMeta.prettyName = newTableTypeSelection;
            const updatedTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(tableMeta);
            setTableMeta(updatedTableMeta);
        }
        setSelection(newTableTypeSelection);
    };

    const deleteAction = async () => {
        await repository.Configuration.Tables.Dynamic.deleteDynamicTable(areaIdentifier, defaultTableMeta.tableType, defaultTableMeta.tableId);
        const newTableMetas = removeByIndex(tableMetas, tableMetaIndex);
        setTableMetas(cloneDeep(newTableMetas));
        changeParentState(!parentState);
        setLoaded(false);
    };

    const metaData: DynamicTableProps | undefined = tableMeta && {
        showDebug: showDebug,
        setTableMeta: setTableMeta,
        tableMeta: tableMeta,
        tableTag: tableTag,
        tableId: tableMeta.tableId,
        tableData: dynamicTableData,
        setTableData: setDynamicTableData,
        areaIdentifier: areaIdentifier,
        deleteAction: deleteAction,
    };

    const DynamicTableComponent = tableMeta && tableMeta.tableType && dynamicTableComponentMap[tableMeta.tableType];

    return (
        <>
            {metaData && (
                <section className={cls.section}>
                    {tableMeta && (
                        <Box className={cls.table}>
                            <DynamicTableSelector
                                toolTipTitle={disabledSelector ? "Disabled when pricing strategy is used in the Palavyr configuration." : ""}
                                disabled={disabledSelector}
                                selection={selection}
                                handleChange={handleChange}
                                tableOptions={availablDynamicTableOptions}
                            />
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
                    )}
                    {tableMeta === undefined && <div>Loading...</div>}
                    {dynamicTableData && DynamicTableComponent && <DynamicTableComponent {...metaData} />}
                </section>
            )}
        </>
    );
};
