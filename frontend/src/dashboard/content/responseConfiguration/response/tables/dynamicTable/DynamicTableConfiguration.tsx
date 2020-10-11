import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect, Suspense } from "react";
import { DynamicTableMetas } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { Accordion, AccordionSummary, Typography, Button, makeStyles } from "@material-ui/core";
import { SingleDynamicFeeTable } from "./SingleDynamicFeeTable";
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

export interface IDynamicTable {
    title: string;
    areaIdentifier: string;
}

const useStyles = makeStyles(theme => ({
    title: {
        fontSize: "28pt"
    },
    header: {
        // background: "#90caf9",
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
        borderTopRightRadius: "8px",
        borderTopLeftRadius: "8px",
    },
}))

export const DynamicTableConfiguration = ({ title, areaIdentifier }: IDynamicTable) => {

    const client = new ApiClient();
    const classes = useStyles();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [parentState, changeParentState] = useState<boolean>(false)

    const [tableMetas, setTableMetas] = useState<DynamicTableMetas>([]);
    const [availableTables, setAvailableTables] = useState<Array<string>>([])

    const loadTableData = useCallback(async () => {

        var res = await client.Configuration.Tables.Dynamic.getDynamicTableMetas(areaIdentifier);
        var availableRes = await client.Configuration.Tables.Dynamic.getAvailableTables()

        var dynamicTableMetas = res.data as DynamicTableMetas;

        setTableMetas(cloneDeep(dynamicTableMetas))
        setAvailableTables(availableRes.data)

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier])

    useEffect(() => {
        if (!loaded) {
            loadTableData();
            setLoaded(true)
        }
        return () => {
            return setLoaded(false)
        }
    }, [areaIdentifier, loadTableData])

    return (
        <>
            <Accordion defaultExpanded>
                <AccordionSummary className={classes.header} expandIcon={<ExpandMoreIcon style={{color: "white"}}/>} aria-controls="panel-content" id="panel-header">
                    <Typography variant="h2" className={classes.title}>{title}</Typography>
                </AccordionSummary>
                <Suspense fallback={<h1>Loading Dynamic Tables...</h1>}>
                    {(tableMetas.length === 0) && <Typography color="secondary" style={{ padding: "0.8rem" }} variant="h5"  >No dynamic tables configured for this area.</Typography>}

                    {
                        tableMetas.map((tableMeta, index) => {
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
                                    parentState={parentState}
                                    changeParentState={changeParentState}
                                    areaIdentifier={areaIdentifier}
                                />
                            )
                        })
                    }

                    <Button
                        variant="contained"
                        color="primary"
                        onClick={async () => {

                            var res = await client.Configuration.Tables.Dynamic.createDynamicTable(areaIdentifier);
                            var newMeta = res.data;
                            tableMetas.push(newMeta);

                            setTableMetas(cloneDeep(tableMetas));

                            changeParentState(!parentState); // TODO: Reload the tables...
                        }}
                    >
                        Add Dynamic Table
                </Button>
                </Suspense>
            </Accordion>
        </>
    );
};
