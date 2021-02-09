import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect, Suspense } from "react";
import { DynamicTableMetas } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Accordion, AccordionSummary, Typography, Button, makeStyles } from "@material-ui/core";
import { SingleDynamicFeeTable } from "./SingleDynamicFeeTable";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import AddBoxIcon from '@material-ui/icons/AddBox';

export interface IDynamicTable {
    title: string;
    areaIdentifier: string;
    children: React.ReactNode;
}

const useStyles = makeStyles((theme) => ({
    title: {
        fontWeight: "bold",
    },
    header: {
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
    },
}));

export type TableNameMap = {
    [tableName: string]: string;
};
export const DynamicTableConfiguration = ({ title, areaIdentifier, children }: IDynamicTable) => {
    const client = new ApiClient();
    const classes = useStyles();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [parentState, changeParentState] = useState<boolean>(false);

    const [tableMetas, setTableMetas] = useState<DynamicTableMetas>([]);
    const [availableTables, setAvailableTables] = useState<Array<string>>([]);
    const [tableNameMap, setTableNameMap] = useState<TableNameMap>({});

    const loadTableData = useCallback(async () => {
        const { data: dynamicTableMetas } = await client.Configuration.Tables.Dynamic.getDynamicTableMetas(areaIdentifier);
        const { data: tableNameMap } = await client.Configuration.Tables.Dynamic.getDynamicTableTypes();

        // map that provides e.g. Select One Flat: SelectOneFlat. used to derive the pretty names
        const availableTablePrettyNames = Object.keys(tableNameMap);

        setTableMetas(cloneDeep(dynamicTableMetas));
        setAvailableTables(availableTablePrettyNames);
        setTableNameMap(tableNameMap);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    const addDynamicTable = async () => {
        // We always add the default dynamic table - the Select One Flat table
        var { data: newMeta } = await client.Configuration.Tables.Dynamic.createDynamicTable(areaIdentifier);
        tableMetas.push(newMeta);
        setTableMetas(cloneDeep(tableMetas));
        changeParentState(!parentState); // TODO: Reload the tables...
    };

    useEffect(() => {
        if (!loaded) {
            loadTableData();
            setLoaded(true);
        }
        return () => {
            return setLoaded(false);
        };
    }, [areaIdentifier, loadTableData]);

    return (
        <>
            <Accordion>
                <AccordionSummary className={classes.header} expandIcon={<ExpandMoreIcon style={{ color: "white" }} />} aria-controls="panel-content" id="panel-header">
                    <Typography className={classes.title}>{title}</Typography>
                </AccordionSummary>
                {children}
                <Suspense fallback={<h1>Loading Dynamic Tables...</h1>}>
                    {tableMetas.length === 0 && (
                        <Typography color="secondary" style={{ padding: "0.8rem" }} variant="h5">
                            No dynamic tables configured for this area.
                        </Typography>
                    )}

                    {tableMetas.map((tableMeta, index) => {
                        return (
                            <SingleDynamicFeeTable
                                key={index}
                                tableNumber={index}
                                setLoaded={setLoaded}
                                tableMetas={tableMetas}
                                setTableMetas={setTableMetas}
                                tableMetaIndex={index}
                                defaultTableMeta={tableMeta}
                                availablDynamicTableOptions={availableTables}
                                tableNameMap={tableNameMap}
                                parentState={parentState}
                                changeParentState={changeParentState}
                                areaIdentifier={areaIdentifier}
                            />
                        );
                    })}
                    <Button startIcon={<AddBoxIcon />} variant="contained" color="primary" style={{ marginTop: "1rem", marginBottom: "1rem", left: "15px", bottom: "10px" }} onClick={addDynamicTable}>
                        Add Dynamic Table
                    </Button>
                </Suspense>
            </Accordion>
        </>
    );
};
