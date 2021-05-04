import React from "react";
import { DynamicTableMeta, DynamicTableMetas, DynamicTableProps, TableData, TableNameMap } from "@Palavyr-Types";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { TextField, makeStyles, Typography, Table, TableRow, TableCell, TableBody } from "@material-ui/core";
import { DynamicTableSelector } from "./DynamicTableSelector";
import { removeByIndex } from "@common/utils";
import { cloneDeep } from "lodash";
import { useState } from "react";
import { useCallback } from "react";
import { useEffect } from "react";
import { ChangeEvent } from "react";
import { dynamicTableComponentMap } from "./DynamicTableRegistry";

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

const useStyles = makeStyles((theme) => ({
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
    },
    section: {
        border: `1px solid ${theme.palette.common.black}`,
        margin: "1.2rem",
        borderRadius: "5px",
        background: theme.palette.secondary.light,
    },
    table: {
        border: "none",
        background: theme.palette.secondary.light,
    },
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
    const repository = new PalavyrRepository();
    const classes = useStyles();

    const [tableMeta, setTableMeta] = useState<DynamicTableMeta | undefined>();
    const [dynamicTableData, setDynamicTableData] = useState<TableData>();
    const [selection, setSelection] = useState<string>(""); // just the node type
    const [tableTag, setTableTag] = useState<string>("");

    const loadDynamicData = useCallback(async () => {
        setTableMeta(defaultTableMeta);
        const tableData = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(areaIdentifier, defaultTableMeta.tableType, defaultTableMeta.tableId);

        setDynamicTableData(tableData);
        setSelection(defaultTableMeta.prettyName);
        setTableTag(defaultTableMeta.tableTag);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        loadDynamicData();
    }, [loadDynamicData, tableMetas]);

    useEffect(() => {
        (async () => {
            if (tableMeta !== undefined) {
                var tableDataResponse = await repository.Configuration.Tables.Dynamic.getDynamicTableRows(areaIdentifier, tableMeta.tableType, tableMeta.tableId);
                setDynamicTableData(tableDataResponse);
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
        var newTableMetas = removeByIndex(tableMetas, tableMetaIndex);
        setTableMetas(cloneDeep(newTableMetas));
        changeParentState(!parentState);
        setLoaded(false);
    };

    const metaData: DynamicTableProps | undefined = tableMeta && {
        showDebug: showDebug,
        setTableMeta: setTableMeta,
        tableMeta: tableMeta,
        tableTag: tableTag,
        tableId: tableMeta?.tableId,
        tableData: dynamicTableData,
        setTableData: setDynamicTableData,
        areaIdentifier: areaIdentifier,
        deleteAction: deleteAction,
    };

    const DynamicTableComponent = tableMeta?.tableType && dynamicTableComponentMap[tableMeta?.tableType];

    return (
        <>
            {metaData && (
                <section className={classes.section}>
                    {tableMeta && (
                        <Table className={classes.table}>
                            <TableBody>
                                <TableRow>
                                    <TableCell>
                                        <DynamicTableSelector selection={selection} handleChange={handleChange} tableOptions={availablDynamicTableOptions} />
                                    </TableCell>
                                    <TableCell></TableCell>
                                    <TableCell></TableCell>
                                    <TableCell align="center">
                                        <TextField
                                            style={{ fontSize: "12pt" }}
                                            fullWidth
                                            label="Short table description (2 or 3 words)"
                                            value={tableTag}
                                            onChange={(e) => {
                                                e.preventDefault();
                                                setTableTag(e.target.value);
                                            }}
                                        />
                                        <Typography className={classes.headerCol}>Custom fee: {tableNumber + 1}</Typography>
                                    </TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    )}
                    {tableMeta === undefined && <div>Loading...</div>}
                    {dynamicTableData && DynamicTableComponent && <DynamicTableComponent {...metaData} />}
                </section>
            )}
        </>
    );
};
