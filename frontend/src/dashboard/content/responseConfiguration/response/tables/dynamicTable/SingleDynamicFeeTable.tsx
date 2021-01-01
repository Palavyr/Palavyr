import React from "react";
import { DynamicTableMeta, DynamicTableMetas } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { TextField, makeStyles, Typography, Table, TableRow, TableCell, TableBody } from "@material-ui/core";
import { DynamicTableSelector } from "./DynamicTableSelector";
import { removeByIndex } from "@common/utils";
import { cloneDeep } from "lodash";
import { DynamicTableTypes, TableData } from "./DynamicTableTypes";
import { SelectOneFlat } from "./tableComponents/SelectOneFlat/SelectOneFlat";
import { TableNameMap } from "./DynamicTableConfiguration";
import { useState } from "react";
import { useCallback } from "react";
import { useEffect } from "react";
import { ChangeEvent } from "react";
import { PercentOfThreshold } from "./tableComponents/PercentOfThreshold/PercentOfThreshold";

export interface ISingleDynamicFeeTable {
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
        border: `3px dashed ${theme.palette.common.black}`,
        margin: "1.2rem",
        borderRadius: "5px",
        background: "#C7ECEE",
    },
    table: {
        border: "none",
        background: "#C7ECEE",
    },
}));

export const SingleDynamicFeeTable = ({ tableNumber, setLoaded, tableMetaIndex, tableMetas, setTableMetas, defaultTableMeta, availablDynamicTableOptions, tableNameMap, parentState, changeParentState, areaIdentifier }: ISingleDynamicFeeTable) => {
    const client = new ApiClient();
    const classes = useStyles();

    const [tableMeta, setTableMeta] = useState<DynamicTableMeta | undefined>();
    const [dynamicTableData, setDynamicTableData] = useState<TableData>();
    const [selection, setSelection] = useState<string>(""); // just the node type
    const [tableTag, setTableTag] = useState<string>("");

    const loadDynamicData = useCallback(async () => {

        setTableMeta(defaultTableMeta);
        var { data: tableData } = await client.Configuration.Tables.Dynamic.getDynamicTableData(areaIdentifier, defaultTableMeta.tableType, defaultTableMeta.tableId);

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
                var { data: tableDataResponse } = await client.Configuration.Tables.Dynamic.getDynamicTableData(areaIdentifier, tableMeta.tableType, tableMeta.tableId);
                setDynamicTableData(tableDataResponse);
            }
        })();

        return () => {};

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, tableMeta, selection]);

    const handleChange = async (event: ChangeEvent<{ name?: string | undefined; value: unknown }>) => {

        const newTableTypeSelection = event.target.value as string; // will be tableType as shown in the list (Select One Flat)
        // this needs to map to the form used in the table dataresponse format (e.g. SelectOneFlat)
        const newTableTypeSelectionFormatted = tableNameMap[newTableTypeSelection]

        if (tableMeta !== undefined) {
            tableMeta.tableType = newTableTypeSelectionFormatted;
            tableMeta.prettyName = newTableTypeSelection;
            const {data: updatedTableMeta} = await client.Configuration.Tables.Dynamic.modifyDynamicTableMeta(tableMeta);
            setTableMeta(updatedTableMeta);
        }
        setSelection(newTableTypeSelection);

    };

    const deleteAction = async () => {
        await client.Configuration.Tables.Dynamic.deleteDynamicTable(areaIdentifier, defaultTableMeta.tableType, defaultTableMeta.tableId);
        var newTableMetas = removeByIndex(tableMetas, tableMetaIndex);
        setTableMetas(cloneDeep(newTableMetas));
        changeParentState(!parentState);
        setLoaded(false);
    };

    return (
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
            {tableMeta?.tableType === DynamicTableTypes.SelectOneFlat && dynamicTableData !== undefined && (
                <SelectOneFlat
                    setTableMeta={setTableMeta}
                    tableMeta={tableMeta}
                    tableTag={tableTag}
                    tableId={tableMeta.tableId}
                    tableData={dynamicTableData}
                    setTableData={setDynamicTableData}
                    areaIdentifier={areaIdentifier}
                    deleteAction={deleteAction}
                />
            )}
            {tableMeta?.tableType === DynamicTableTypes.PercentOfThreshold && dynamicTableData !== undefined && (
                <PercentOfThreshold
                    setTableMeta={setTableMeta}
                    tableMeta={tableMeta}
                    tableTag={tableTag}
                    tableId={tableMeta.tableId}
                    tableData={dynamicTableData}
                    setTableData={setDynamicTableData}
                    areaIdentifier={areaIdentifier}
                    deleteAction={deleteAction}
                />
            )}
        </section>
    );
};
