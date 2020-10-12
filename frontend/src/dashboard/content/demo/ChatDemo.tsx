import { AreaTable } from "@Palavyr-Types";
import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { Grid, Paper, Typography, makeStyles, Divider, TextField, TableRow, TableCell, TableBody, Table, TableHead } from "@material-ui/core";
import { widgetUrl } from "@api-client/clientUtils";
import classNames from "classnames";
import { SaveOrCancel } from "@common/components/SaveOrCancel";


export type PreCheckResult = {
    isReady: boolean;
    incompleteAreas: Array<AreaTable>
}

export type IncompleteAreas = {
    areaDisplayTitle: string;
    areaName: string;
}

export type WidgetPreferences = {
    title: string;
    subtitle: string;
    placeholder: string;
}

const useStyles = makeStyles(theme => ({
    formroot: {
        display: 'flex',
        flexWrap: 'wrap',
    },
    paper: {
        alignItems: "center",
        // marginTop: "4rem",
        backgroundColor: "#535c68",
        border: "0px solid black",
        boxShadow: "0 0 black",
    },
    grid: {
        border: "0px solid black"
    },

    frame: props => ({
        marginTop: props ? "0rem" : "2rem",
        marginBottom: props ? "0rem" : "2rem",
        height: "500px",
        width: "380px",
        borderRadius: "9px",
        border: "0px"
    }),
    container: {
        height: "100%"
    },
    widgetcell: {
        borderRight: "2px solid black",
        height: "100%",
        textAlign: "center",
        backgroundColor: "#535c68",
        paddingBottom: "2rem"
    },
    lowercell: {
        paddingTop: "1rem",
        // textAlign: "center",
        borderTop: "4px solid black",
        // justify: "center",
        // alignItems: "center"
    },
    table: {
        width: '55%',
        border: "0px solid black",

    }
}))

interface IChatDemo {
    selectHelpDrawerContent: any;
}

export const ChatDemo = ({ selectHelpDrawerContent }: IChatDemo) => {
    var client = new ApiClient();

    selectHelpDrawerContent("demo");

    const [incompleteAreas, setIncompleteAreas] = useState<Array<IncompleteAreas>>([]);
    const [apiKey, setApiKey] = useState<string>("");

    // widget preferences
    const [title, setTitle] = useState<string>("");
    const [subTitle, setSubTitle] = useState<string>("");
    const [placeholder, setPlaceholder] = useState<string>("");

    const classes = useStyles(incompleteAreas.length > 0);

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

    const savePrefs = useCallback(async () => {

        const prefs: WidgetPreferences = {
            title: title,
            subtitle: subTitle,
            placeholder: placeholder
        }

        var res = await client.WidgetDemo.SaveWidgetPreferences(prefs);
    }, [])

    useEffect(() => {
        loadMissingNodes();
    }, [loadMissingNodes])

    const loadApiKey = useCallback(async () => {
        var res = await client.Settings.Account.getApiKey();
        var apiKey = res.data as string;
        setApiKey(apiKey);
    }, [])

    const loadPrefs = useCallback(async () => {
        var prefs = (await client.WidgetDemo.GetWidetPreferences()).data as WidgetPreferences;
        setTitle(prefs.title);
        setSubTitle(prefs.subtitle);
        setPlaceholder(prefs.placeholder);
    }, [])

    useEffect(() => {
        loadApiKey();
        loadPrefs();
    }, [loadApiKey, loadPrefs])

    return (
        <Grid className={classNames(classes.grid, classes.container)} container>
            <Grid justify="center" className={classNames(classes.grid, classes.widgetcell)} container xs={6}>
                <Paper className={classes.paper} >
                    {
                        incompleteAreas.length > 0
                        && <Typography style={{ paddingTop: "2rem", paddingBottom: "2rem", color: "white"}}>The Demo will load once you've fully assembled each of your areas!</Typography>
                    }
                    <div>
                        {apiKey && <iframe title="demo" className={classes.frame} src={`${widgetUrl}/widget/${apiKey}`}></iframe>}
                    </div>
                </Paper>
            </Grid>
            <Grid className={classes.grid} container xs={6}>
                <Typography align="center" style={{ alignSelf: "center", padding: "0px" }} variant="h5">Customize your widget</Typography>
                <Paper className={classes.formroot}>
                    <TextField
                        id="standard-full-width"
                        style={{ margin: 3 }}
                        placeholder=""
                        helperText="Title"
                        fullWidth
                        margin="normal"
                        InputLabelProps={{
                            shrink: true,
                        }}
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                    />
                    <TextField
                        id="standard-full-width"
                        style={{ margin: 3 }}
                        placeholder=""
                        helperText="Subtitle"
                        fullWidth
                        margin="normal"
                        InputLabelProps={{
                            shrink: true,
                        }}
                        value={subTitle}
                        onChange={(e) => setSubTitle(e.target.value)}
                    />
                    <TextField
                        id="standard-full-width"
                        style={{ margin: 3 }}
                        placeholder=""
                        helperText="Placeholder"
                        fullWidth
                        margin="normal"
                        InputLabelProps={{
                            shrink: true,
                        }}
                        value={placeholder}
                        onChange={(e) => setPlaceholder(e.target.value)}
                    />
                    <SaveOrCancel
                        size="small"
                        onSave={() => savePrefs()}
                    />
                </Paper>
            </Grid>
            <Grid className={classes.lowercell} xs={12} >
                <Grid item xs={12}>
                    <Typography style={{paddingBottom: "1rem"}} align="center" variant="h5">Areas to configure</Typography>
                </Grid>
                <Grid item xs={12} alignContent="center" justify="center">
                    <Table className={classes.table}>
                        <TableHead>
                            <TableRow>
                                <TableCell align="center"><Typography variant="h6">Area Name</Typography></TableCell>
                                <TableCell align="center"><Typography variant="h6">Area Title</Typography></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {
                                incompleteAreas.length > 0 &&
                                (
                                    incompleteAreas.map((area, index) => {
                                        return (
                                            <TableRow>
                                                <TableCell align="center">{area.areaName}</TableCell>
                                                <TableCell align="center">{area.areaDisplayTitle}</TableCell>
                                            </TableRow>
                                        )

                                    })
                                )
                            }
                        </TableBody>
                    </Table>
                </Grid>
            </Grid>
        </Grid>
    )
}