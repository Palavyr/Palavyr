import { DynamicTableMeta, DynamicTableMetas } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import React from "react";
import { Grid, TextField, makeStyles, Typography, Table, TableHead, TableRow, TableCell, TableBody } from "@material-ui/core";
import { DynamicTableSelector } from "./DynamicTableSelector";
import { removeByIndex } from "@common/utils";
import { cloneDeep } from "lodash";
import { DynamicTableTypes } from "./DynamicTableTypes";
import { SelectOneFlatTable } from "./tableComponents/SelectOneFlat/SelectOneFlat";
import classNames from "classnames";


export interface ISingleDynamicFeeTable {
    defaultTableMeta: DynamicTableMeta;
    availablDynamicTableOptions: Array<string>;
    parentState: boolean;
    changeParentState: any;
    areaIdentifier: string;
    tableMetaIndex: number;
    tableMetas: DynamicTableMetas;
    setTableMetas: any;
    setLoaded: any;
    tableNumber: number;
}

const useStyles = makeStyles((theme => ({
    headerCol: {
        textAlign: "center"
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
        background: "#E8E8E8"
    },
    table: {
        border: "none",
        background: "#E8E8E8"

    }
})))

export const SingleDynamicFeeTable = ({ tableNumber, setLoaded, tableMetaIndex, tableMetas, setTableMetas, defaultTableMeta, availablDynamicTableOptions, parentState, changeParentState, areaIdentifier }: ISingleDynamicFeeTable) => {

    const client = new ApiClient();

    const classes = useStyles();


    const [tableMeta, setTableMeta] = React.useState<DynamicTableMeta | undefined>()
    const [dynamicTableData, setDynamicTableData] = React.useState<any>();
    const [selection, setSelection] = React.useState<string>(""); // just the node type
    const [tableTag, setTableTag] = React.useState<string>("");

    const loadDynamicData = React.useCallback(async () => {

        setTableMeta(defaultTableMeta);
        var res = await client.Configuration.Tables.Dynamic.getDynamicTableData(areaIdentifier, defaultTableMeta.tableType, defaultTableMeta.tableId)
        var tableData = res.data;
        setDynamicTableData(tableData);
        setSelection(defaultTableMeta.prettyName)
        setTableTag(defaultTableMeta.tableTag);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    React.useEffect(() => {
        loadDynamicData();
    }, [loadDynamicData, tableMetas])


    React.useEffect(() => {

        (async () => {
            if (tableMeta !== undefined) {
                var tableDataResponse = await client.Configuration.Tables.Dynamic.getDynamicTableData(areaIdentifier, tableMeta.tableType, tableMeta.tableId);
                setDynamicTableData(tableDataResponse.data);
            }
        })()

        return () => { }

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier, tableMeta])


    const handleChange = async (event: React.ChangeEvent<{ name?: string | undefined; value: unknown }>) => {
        const newTableTypeSelection = event.target.value as string; // will be tableType
        var tableDataResponse = await client.Configuration.Tables.Dynamic.getDynamicTableData(areaIdentifier, newTableTypeSelection, tableMeta!.tableId)
        setDynamicTableData(tableDataResponse.data);
    };

    const deleteAction = async () => {
        await client.Configuration.Tables.Dynamic.deleteDynamicTable(areaIdentifier, defaultTableMeta.tableType, defaultTableMeta.tableId)
        var newTableMetas = removeByIndex(tableMetas, tableMetaIndex);
        setTableMetas(cloneDeep(newTableMetas));
        changeParentState(!parentState);
        setLoaded(false);
    }

    return (
        <section className={classes.section} >

            {
                tableMeta &&
                <Table className={classes.table}>
                    <TableBody >
                        <TableRow>
                            <TableCell>
                                <DynamicTableSelector
                                    selection={selection}
                                    setSelection={setSelection}
                                    handleChange={handleChange}
                                    currentTableMeta={defaultTableMeta}
                                    tableOptions={availablDynamicTableOptions}
                                    parentState={parentState}
                                    changeParentState={changeParentState}
                                    areaIdentifier={areaIdentifier}
                                />
                            </TableCell>
                            <TableCell></TableCell>
                            <TableCell align="center">
                                <Typography className={classes.headerCol}>
                                    Custom fee: {tableNumber + 1}
                                </Typography>
                            </TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            }
            {(tableMeta === undefined) && <div>Loading...</div>}
            {
                (tableMeta?.tableType === DynamicTableTypes.SelectOneFlat)
                && (dynamicTableData !== undefined)
                && <SelectOneFlatTable
                    setTableMeta={setTableMeta}
                    tableMeta={tableMeta}
                    tableTag={tableTag}
                    tableId={tableMeta.tableId}
                    tableData={dynamicTableData}
                    setTableData={setDynamicTableData}
                    areaIdentifier={areaIdentifier}
                    deleteAction={deleteAction}
                />
            }

        </section>
    )
}