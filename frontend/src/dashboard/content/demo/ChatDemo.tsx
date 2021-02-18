import { AreaTable, IncompleteAreas, WidgetPreferences } from "@Palavyr-Types";
import React, { useState, useCallback, useEffect, Dispatch, SetStateAction } from "react";
import { ApiClient } from "@api-client/Client";
import { Grid, Paper, Typography, makeStyles, Divider, GridList, GridListTile } from "@material-ui/core";
import { widgetUrl } from "@api-client/clientUtils";
import classNames from "classnames";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { HeaderEditor } from "./HeaderEditor";
import { ChromePicker } from "react-color";
import { IFrame } from "./IFrame";
import { CustomSelect } from "../responseConfiguration/response/tables/dynamicTable/CustomSelect";
import { AreasInNeedOfAttention } from "./AreasInNeedOfAttention";
import { DemoTextInput } from "./DemoTextInput";
import { ChatDemoHeader } from "./ChatDemoHeader";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

const useStyles = makeStyles((theme) => ({
    formroot: {
        display: "flex",
        flexWrap: "wrap",
        width: "100%",
        paddingLeft: "1.4rem",
        paddingRight: "2.3rem",
        justifyContent: "center",
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
        justifyContent: "center",
    },
    container: {
        height: "100%",
    },
    widgetcell: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        borderRight: "2px solid black",
        textAlign: "center",
        backgroundColor: "#535c68",
    },

    lowercell: {
        borderBottom: "4px solid black",
    },

    actions: {
        width: "100%",
        display: "flex",
        padding: "8px",
        alignItems: "center",
        justifyContent: "flex-start",
    },
    editorContainer: {
        width: "100%",
    },

    centerText: {
        textAlign: "center",
        justifyContent: "flex-end",
        alignSelf: "center",
        alignItems: "center",
    },

    root: {
        display: "flex",
        flexWrap: "wrap",
        justifyContent: "space-around",
        overflow: "hidden",
        backgroundColor: theme.palette.background.paper,
    },
    gridList: {
        width: "100%",
        height: "100%",
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
    },
    gridListTile: {
        margin: "1rem",
    },
    singleGridListTile: {
        margin: "1rem",
    },
    pickerTitle: {
        marginBottom: "0.3rem",
    },
}));

type ColorPickerType = {
    method: Dispatch<SetStateAction<string>>;
    name: string;
    variable: string;
    disable: boolean;
};

export const ChatDemo = () => {
    var client = new ApiClient();

    const [incompleteAreas, setIncompleteAreas] = useState<IncompleteAreas>([]);
    const [apiKey, setApiKey] = useState<string>("");
    const [iframeRefreshed, reloadIframe] = useState<boolean>(false);

    // widget preferences
    const [header, setHeader] = useState<string>("");
    const [initialHeader, setInitialHeader] = useState<string>("");
    const [selectListColor, setListColor] = useState<string>("");
    const [headerColor, setHeaderColor] = useState<string>("");
    const [fontFamily, setFontFamily] = useState<string>("");
    const [title, setTitle] = useState<string>("");
    const [subTitle, setSubTitle] = useState<string>("");
    const [placeholder, setPlaceholder] = useState<string>("");

    const [listFontColor, setListFontColor] = useState<string>("");
    const [headerFontColor, setHeaderFontColor] = useState<string>("");
    const [optionsHeaderColor, setOptionsHeaderColor] = useState<string>("");
    const [optionsHeaderFontColor, setOptionsHeaderFontColor] = useState<string>("");
    const [chatFontColor, setChatFontColor] = useState<string>("");
    const [chatBubbleColor, setChatBubbleColor] = useState<string>("");

    const cls = useStyles(incompleteAreas.length > 0);
    const { setIsLoading } = React.useContext(DashboardContext);

    const loadMissingNodes = useCallback(async () => {
        const { data: PreCheckResult } = await client.WidgetDemo.RunConversationPrecheck();
        if (!PreCheckResult.isReady) {
            const areas = PreCheckResult.incompleteAreas.map((x: AreaTable) => {
                return {
                    areaDisplayTitle: x.areaDisplayTitle,
                    areaName: x.areaName,
                };
            });
            setIncompleteAreas(areas);
        }
    }, []);

    const savePrefs = async () => {
        const prefs: WidgetPreferences = {
            selectListColor: selectListColor,
            headerColor: headerColor,
            headerFontColor: headerFontColor,
            fontFamily: fontFamily,
            header: header,
            title: title,
            subtitle: subTitle,
            placeholder: placeholder,
            listFontColor: listFontColor,
            optionsHeaderColor: optionsHeaderColor,
            optionsHeaderFontColor: optionsHeaderFontColor,
            chatFontColor: chatFontColor,
            chatBubbleColor: chatBubbleColor,
        };
        const { data } = await client.WidgetDemo.SaveWidgetPreferences(prefs);
        reloadIframe(!iframeRefreshed);
    };

    useEffect(() => {
        loadMissingNodes();
    }, [loadMissingNodes]);

    const loadDemoWidget = useCallback(async () => {
        setIsLoading(true);
        const { data: key } = await client.Settings.Account.getApiKey();
        setApiKey(key);

        const { data: prefs } = await client.WidgetDemo.GetWidetPreferences();
        const { header, selectListColor, headerColor, fontFamily, title, subtitle, placeholder, listFontColor, headerFontColor, optionsHeaderColor, optionsHeaderFontColor, chatFontColor, chatBubbleColor } = prefs;

        setInitialHeader(header);
        setListColor(selectListColor);
        setHeaderColor(headerColor);
        setFontFamily(fontFamily);
        setTitle(title);
        setSubTitle(subtitle);
        setPlaceholder(placeholder);
        setListFontColor(listFontColor);
        setHeaderFontColor(headerFontColor);
        setOptionsHeaderColor(optionsHeaderColor);
        setOptionsHeaderFontColor(optionsHeaderFontColor);
        setChatFontColor(chatFontColor);
        setChatBubbleColor(chatBubbleColor);
        setIsLoading(false);

    }, []);

    useEffect(() => {
        loadDemoWidget();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const supportedFonts = ["Architects Daughter"];

    const textPickers = [
        { name: "Title", variable: title, method: setTitle, disable: false },
        { name: "Subtitle", variable: subTitle, method: setSubTitle, disable: false },
        { name: "Placeholder", variable: placeholder, method: setPlaceholder, disable: false },
    ]

    const colorPickers: ColorPickerType[][] = [
        [
            { name: "Header Color", variable: headerColor, method: setHeaderColor, disable: true },
            { name: "Header Font Color", variable: headerFontColor, method: setHeaderFontColor, disable: true },
        ],
        [
            { name: "Options List Color", variable: selectListColor, method: setListColor, disable: true },
            { name: "Options List Font Color", variable: listFontColor, method: setListFontColor, disable: true },
        ],
        [
            { name: "Chat Bubble Color", variable: chatBubbleColor, method: setChatBubbleColor, disable: true},
            { name: "Chat Bubble Font Color", variable: chatFontColor, method: setChatFontColor, disable: true },
        ],
    ];

    return (
        <>
            {incompleteAreas.length > 0 && <AreasInNeedOfAttention incompleteAreas={incompleteAreas} />}
            <ChatDemoHeader />
            <Divider />
            <Grid container className={classNames(cls.grid, cls.container, cls.lowercell)}>
                <Grid className={classNames(cls.grid, cls.widgetcell)} item xs={6}>
                    <Paper className={cls.paper}>
                        {incompleteAreas.length > 0 && <Typography style={{ paddingTop: "2rem", paddingBottom: "2rem", color: "white" }}>The Demo will load once you've fully assembled each of your areas!</Typography>}
                        <div>{apiKey && <IFrame widgetUrl={widgetUrl} apiKey={apiKey} iframeRefreshed={iframeRefreshed} incompleteAreas={incompleteAreas} />}</div>
                    </Paper>
                </Grid>

                <Grid container className={classNames(cls.grid)}>
                    <Paper className={cls.formroot}>
                        <div className={cls.editorContainer}>
                            <HeaderEditor setEditorState={setHeader} initialData={initialHeader} label="Header" />
                        </div>
                        <Grid item xs={6}>
                            {textPickers.map((picker: ColorPickerType) => (
                                <DemoTextInput disabled={picker.disable} text={picker.name} value={picker.variable} onChange={(e) => picker.method(e.target.value)} />
                            ))}
                            <CustomSelect
                                option={fontFamily}
                                options={supportedFonts}
                                helperText="Font Family"
                                width="50%"
                                align="left"
                                onChange={(event) => {
                                    const newFont = event.target.value as string;
                                    setFontFamily(newFont);
                                }}
                            />
                            <div className={cls.actions}>
                                <SaveOrCancel size="large" onSave={() => savePrefs()} />
                            </div>
                        </Grid>
                        <Grid item xs={6} className={cls.centerText}>
                            <div className={cls.root}>
                                <GridList cellHeight={275} className={cls.gridList}>
                                    {colorPickers.map((picker: ColorPickerType[], index: number) => {


                                        if (picker.length == 1) {
                                            const pickerA = picker[0];

                                            return (
                                                <GridListTile cols={1} key={pickerA.name} className={cls.singleGridListTile}>
                                                    <Typography align="left" variant="body1" className={cls.pickerTitle}>
                                                        {pickerA.name}
                                                    </Typography>
                                                    {pickerA.variable && <ChromePicker disableAlpha color={pickerA.variable} onChangeComplete={(color) => pickerA.method(color.hex)} />}
                                                </GridListTile>
                                            );
                                        } else {
                                            const pickerA = picker[0];
                                            const pickerB = picker[1];
                                            return (
                                                <>
                                                    <GridListTile cols={2} key={pickerA.name} className={cls.gridListTile}>
                                                        <Typography align="center" variant="body1" className={cls.pickerTitle}>
                                                            {pickerA.name}
                                                        </Typography>
                                                        {pickerA.variable && <ChromePicker disableAlpha color={pickerA.variable} onChangeComplete={(color) => pickerA.method(color.hex)} />}
                                                    </GridListTile>
                                                    <GridListTile cols={2} key={pickerB.name} className={cls.gridListTile}>
                                                        <Typography align="center" variant="body1" className={cls.pickerTitle}>
                                                            {pickerB.name}
                                                        </Typography>
                                                        {pickerB.variable && <ChromePicker disableAlpha color={pickerB.variable} onChangeComplete={(color) => pickerB.method(color.hex)} />}
                                                    </GridListTile>
                                                </>
                                            );
                                        }
                                    })}
                                </GridList>
                            </div>
                        </Grid>
                    </Paper>
                </Grid>
            </Grid>
        </>
    );
};
