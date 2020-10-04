import { AreaTable } from "@Palavyr-Types";
import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { Grid, Paper, Typography, makeStyles } from "@material-ui/core";
import { widgetUrl } from "@api-client/clientUtils";


export type PreCheckResult = {
    isReady: boolean;
    incompleteAreas: Array<AreaTable>
}

export type IncompleteAreas = {
    areaDisplayTitle: string;
    areaName: string;
}


const useStyles = makeStyles(theme => ({
    paper: {
        height: "80%",
        widgth: "50%",
        alignItems: "center"
    },
    frame: {
        height: "500px",
        width: "320px",
        borderRadius: "9px",
        border: "0px"
    }
}))

export const ChatDemo = () => {
    var client = new ApiClient();

    const [incompleteAreas, setIncompleteAreas] = useState<Array<IncompleteAreas>>([]);
    const [apiKey, setApiKey] = useState<string>("");

    const classes = useStyles();

    const loadMissingNodes = useCallback(async () => {
        var res = await client.WidgetDemo.RunConversationPrecheck();        
        var PreCheckResult = res.data as PreCheckResult;
        if (!PreCheckResult.isReady) {
            var areas = PreCheckResult.incompleteAreas.map((x: AreaTable) => {
                return {
                    areaDisplayTitle: x.areaDisplayTitle,
                    areaName: x.areaName
                }
            })
            setIncompleteAreas(areas)
        }


    }, [])

    useEffect(() => {
        loadMissingNodes();
    }, [loadMissingNodes])


    const loadApiKey = useCallback(async () => {
        var res = await client.Settings.Account.getApiKey();
        var apiKey = res.data as string;
        setApiKey(apiKey);

    }, [])


    useEffect(() => {
        loadApiKey();
    }, [loadApiKey])


    return (
        <>
            <Grid
                container
                direction="column"
                alignItems="center"
            >
                <Paper className={classes.paper} >
                    <div>
                        <iframe title="demo" className={classes.frame} src={`${widgetUrl}/widget/${apiKey}`}></iframe>
                    </div>
                </Paper>
            </Grid>
            {incompleteAreas.length > 0 && (
                <>
                    <Grid
                        container
                        direction="column"
                        alignItems="center"
                    >
                        <Typography>The Demo will load once you've fully assembled each of your areas!</Typography>
                        {
                            incompleteAreas.map((area, index) => {
                                return (
                                    <Grid key={index} item>
                                        Name: {area.areaName}
                                Title: {area.areaDisplayTitle}
                                    </Grid>
                                )
                            })
                        }
                    </Grid>
                </>
            )}
        </>
    )
}