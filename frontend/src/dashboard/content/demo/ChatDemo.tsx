import { AreaTable, IGetHelp } from "@Palavyr-Types";
import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { Grid, Paper, Typography, makeStyles, Divider, TextField, TableRow, TableCell, TableBody, Table, TableHead } from "@material-ui/core";
import { widgetUrl } from "@api-client/clientUtils";
import classNames from "classnames";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { HeaderEditor } from "./HeaderEditor";
import { ChromePicker } from 'react-color';
import { IFrame } from "./IFrame";


export type PreCheckResult = {
    isReady: boolean;
    incompleteAreas: Array<AreaTable>
}

export type IncompleteAreas = {
    areaDisplayTitle: string;
    areaName: string;
}
export type WidgetPreferences = {
    selectListColor: string;
    headerColor: string;
    fontFamily: string;
    header: string;
    title: string;
    subtitle: string;
    placeholder: string;
}

const useStyles = makeStyles(theme => ({
    formroot: {
        display: 'flex',
        flexWrap: 'wrap',
        width: "100%",
        paddingLeft: "1.4rem",
        paddingRight: "2.3rem",
        justifyContent: "center"
    },
    paper: {
        alignItems: "center",
        backgroundColor: "#535c68",
        border: "0px solid black",
        boxShadow: "0 0 black",
    },
    grid: {
        border: "0px solid black",
        display: "flex",
        justifyContent: "center"
    },
    container: {
        height: "100%"
    },
    widgetcell: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        borderRight: "2px solid black",
        textAlign: "center",
        backgroundColor: "#535c68",
    },
    uppercell: {
        paddingTop: "1rem",
        paddingBottom: "2rem",
        borderTop: "4px solid black",
        borderBottom: "4px solid black",
    },
    lowercell: {
        borderBottom: "4px solid black",
    },
    table: {
        border: "0px solid black",
    },
    actions: {
        width: "100%",
        display: "flex",
        padding: "8px",
        alignItems: "center",
        justifyContent: "flex-start",
    },
    editorContainer: {
        width: "100%"
    },
    customizetext: {
        paddingTop: "1.8rem",
        paddingBottom: "1.8rem"
    },
    tablegrid: {
        paddingRight: "20%",
        paddingLeft: "20%"
    },
    cell: {
        borderBottom: "1px solid lightgray"
    },
    centerText: {
        textAlign: "center",
        justifyContent: "flex-end",
        alignSelf: "center",
        alignItems: "center"
    },
    div: {
        display: "flex",
        flexDirection: "column",

    },
    colorpicker: {
        paddingBottom: "1rem",
        marginTop: "-2rem",
        marginLeft: "1.2rem",
        borderLeft: "1px solid black",
        display: "flex",
        flexDirection: "column",
        alignItems: "center"
    }
}))

interface IChatDemo extends IGetHelp {}

export const ChatDemo = () => {
    var client = new ApiClient();

    const [incompleteAreas, setIncompleteAreas] = useState<Array<IncompleteAreas>>([]);
    const [apiKey, setApiKey] = useState<string>("");

    // widget preferences
    const [header, setHeader] = useState<string>("");
    const [initialHeader, setInitialHeader] = useState<string>("");

    const [selectListColor, setListColor] = useState<string>("");
    const [headerColor, setHeaderColor] = useState<string>("");
    const [fontFamily, setFontFamily] = useState<string>("");
    const [title, setTitle] = useState<string>("");
    const [subTitle, setSubTitle] = useState<string>("");
    const [placeholder, setPlaceholder] = useState<string>("");
    const [iframeRefreshed, reloadIframe] = useState<boolean>(false);

    const classes = useStyles(incompleteAreas.length > 0);

    const loadMissingNodes = useCallback(async () => {
        var PreCheckResult = (await client.WidgetDemo.RunConversationPrecheck()).data as PreCheckResult;
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

    const savePrefs = async () => {
        const prefs: WidgetPreferences = {
            selectListColor: selectListColor,
            headerColor: headerColor,
            fontFamily: fontFamily,
            header: header,
            title: title,
            subtitle: subTitle,
            placeholder: placeholder
        }
        var res = await client.WidgetDemo.SaveWidgetPreferences(prefs);
        reloadIframe(!iframeRefreshed);

    }

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
        setInitialHeader(prefs.header);
        setListColor(prefs.selectListColor);
        setHeaderColor(prefs.headerColor);
        setFontFamily(prefs.fontFamily);
        setTitle(prefs.title);
        setSubTitle(prefs.subtitle);
        setPlaceholder(prefs.placeholder);
    }, [])

    useEffect(() => {
        loadApiKey();
        loadPrefs();
    }, [loadApiKey, loadPrefs])

    return (
        <>
            {
                incompleteAreas.length > 0 &&
                <Grid className={classes.uppercell} >
                    <Grid className={classes.tablegrid}>
                        <Typography style={{ paddingBottom: "1rem" }} align="center" variant="h6" >Areas in need of attention:</Typography>
                        <Table className={classes.table}>
                            <TableHead>
                                <TableRow>
                                    <TableCell className={classes.cell} width="50%" align="center"><Typography variant="h6">Area Name</Typography></TableCell>
                                    <TableCell className={classes.cell} width="50%" align="center"><Typography variant="h6">Area Title</Typography></TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {
                                    incompleteAreas.map((area, index) => {
                                        return (
                                            <TableRow>
                                                <TableCell key={area.areaName} className={classes.cell} width="50%" align="center"><Typography>{area.areaName}</Typography></TableCell>
                                                <TableCell key={index} className={classes.cell} width="50%" align="center"><Typography>{area.areaDisplayTitle}</Typography></TableCell>
                                            </TableRow>
                                        )

                                    })
                                }
                            </TableBody>
                        </Table>
                    </Grid>
                </Grid>
            }
            <Grid className={classNames(classes.grid, classes.container, classes.lowercell)} container>
                <Grid className={classNames(classes.grid, classes.widgetcell)} item xs={6}>
                    <Paper className={classes.paper} >
                        {
                            incompleteAreas.length > 0
                            && <Typography style={{ paddingTop: "2rem", paddingBottom: "2rem", color: "white" }}>The Demo will load once you've fully assembled each of your areas!</Typography>
                        }
                        <div>
                            {apiKey && <IFrame widgetUrl={widgetUrl} apiKey={apiKey} iframeRefreshed={iframeRefreshed} incompleteAreas={incompleteAreas} />}
                        </div>
                    </Paper>
                </Grid>

                <Grid className={classNames(classes.grid)} container>
                    <Paper className={classes.formroot}>
                        <Typography variant="h4" className={classes.customizetext}>Customize your widget</Typography>
                        <div className={classes.editorContainer}>
                            <HeaderEditor setEditorState={setHeader} initialData={initialHeader} label="Header" />
                        </div>
                        <Grid item xs={6}>
                            <TextField
                                id="standard-full-width-title"
                                style={{ margin: 3, marginBottom: "1.6rem", marginTop: "1rem" }}
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
                                id="standard-full-width-subtitle"
                                style={{ margin: 3, marginBottom: "1.6rem" }}
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
                                id="standard-full-width-placeholder"
                                style={{ margin: 3, marginBottom: "1.6rem" }}
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

                            <TextField
                                id="standard-full-width-optionslistcolor"
                                style={{ margin: 3, marginBottom: "1.6rem", alignSelf: "flex-start" }}
                                placeholder=""
                                helperText="Options List Color"
                                fullWidth
                                margin="normal"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                                value={selectListColor}
                                onChange={(e) => setListColor(e.target.value)}
                            />
                            <TextField
                                id="standard-full-width-header-color"
                                style={{ margin: 3, marginBottom: "1.6rem" }}
                                placeholder=""
                                helperText="Header Color"
                                fullWidth
                                margin="normal"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                                value={headerColor}
                                onChange={(e) => setHeaderColor(e.target.value)}
                            />
                            <TextField
                                id="standard-full-width-fontfamily"
                                style={{ margin: 3, marginBottom: "1.6rem" }}
                                placeholder=""
                                helperText="Font Family"
                                fullWidth
                                margin="normal"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                                value={fontFamily}
                                onChange={(e) => setFontFamily(e.target.value)}
                            />
                            <div className={classes.actions}>
                                <SaveOrCancel
                                    size="large"
                                    onSave={() => savePrefs()}
                                />
                            </div>
                        </Grid>
                        <Grid item xs={6} className={classes.centerText}>
                            <div className={classes.colorpicker}>
                                <div className={classes.div}>
                                    <Typography variant="h6">Options Menu Color</Typography>
                                    <ChromePicker disableAlpha color={selectListColor} onChangeComplete={(color) => setListColor(color.hex)} />
                                </div>
                                <div className={classes.div} style={{ paddingTop: "1rem" }}>
                                    <Typography variant="h6">Chat Header Color</Typography>
                                    <ChromePicker disableAlpha color={headerColor} onChangeComplete={(color) => setHeaderColor(color.hex)} />
                                </div>
                            </div>
                        </Grid>

                    </Paper>
                </Grid>
            </Grid>
        </>
    )
}