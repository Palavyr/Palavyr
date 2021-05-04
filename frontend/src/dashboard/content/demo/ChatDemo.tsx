import { PreCheckError, SetState, WidgetPreferences } from "@Palavyr-Types";
import React, { useState, useCallback, useEffect } from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { Grid, Paper, Typography, makeStyles, Divider } from "@material-ui/core";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { HeaderEditor } from "./HeaderEditor";
import { ChromePicker } from "react-color";
import { CustomSelect } from "../responseConfiguration/response/tables/dynamicTable/CustomSelect";
import { AreasInNeedOfAttention } from "./AreasInNeedOfAttention";
import { ChatDemoHeader } from "./ChatDemoHeader";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { PalavyrDemoWidget } from "./DemoWidget";
import { Align } from "dashboard/layouts/positioning/Align";
import { FakeWidgets } from "./fakeWidget/FakeWidgets";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";

const useStyles = makeStyles((theme) => ({
    gridList: {
        width: "100%",
        height: "100%",
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        paddingLeft: "2rem",
        paddingRight: "2rem",
        paddingBottom: "4rem",
    },

    pickerTitle: {
        marginBottom: "0.3rem",
    },
    paper: {
        padding: theme.spacing(5),
        marginTop: theme.spacing(3),
    },
    colorstext: {
        paddingTop: "1.2rem",
        paddingBottom: "1.2rem",
    },
}));

export type ColorPickerType = {
    method: SetState<string>;
    name: string;
    variable: string;
    disable: boolean;
};

export const ChatDemo = () => {
    const repository = new PalavyrRepository();

    const [preCheckErrors, setPreCheckErrors] = useState<PreCheckError[]>([]);
    const [apiKey, setApiKey] = useState<string>("");
    const [iframeRefreshed, reloadIframe] = useState<boolean>(false);
    const [widgetPreferences, setWidgetPreferences] = useState<WidgetPreferences>();

    const { setIsLoading } = React.useContext(DashboardContext);
    const cls = useStyles(preCheckErrors.length > 0);

    const loadMissingNodes = useCallback(async () => {
        const preCheckResult = await repository.WidgetDemo.RunConversationPrecheck();
        if (!preCheckResult.isReady) {
            setPreCheckErrors(preCheckResult.preCheckErrors);
        }
    }, []);

    const saveWidgetPreferences = async () => {
        if (widgetPreferences) {
            const updatedPreferences = await repository.WidgetDemo.SaveWidgetPreferences(widgetPreferences);
            setWidgetPreferences(updatedPreferences);
            reloadIframe(!iframeRefreshed);
            return true;
        } else {
            return false;
        }
    };

    useEffect(() => {
        loadMissingNodes();
    }, [loadMissingNodes]);

    const loadDemoWidget = useCallback(async () => {
        setIsLoading(true);
        const key = await repository.Settings.Account.getApiKey();
        setApiKey(key);

        const currentWidgetPreferences = await repository.WidgetDemo.GetWidetPreferences();
        setWidgetPreferences(currentWidgetPreferences);

        setIsLoading(false);
    }, []);

    useEffect(() => {
        loadDemoWidget();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const supportedFonts = ["Architects Daughter"];

    const colorPickers = (widgetPreferences: WidgetPreferences): ColorPickerType[] => {
        return [
            { name: "Header Color", variable: widgetPreferences.headerColor, method: (headerColor: string) => setWidgetPreferences({ ...widgetPreferences, headerColor }), disable: true },
            { name: "Header Font Color", variable: widgetPreferences.headerFontColor, method: (headerFontColor: string) => setWidgetPreferences({ ...widgetPreferences, headerFontColor }), disable: true },
            { name: "Options List Color", variable: widgetPreferences.selectListColor, method: (selectListColor: string) => setWidgetPreferences({ ...widgetPreferences, selectListColor }), disable: true },
            { name: "Options List Font Color", variable: widgetPreferences.listFontColor, method: (listFontColor: string) => setWidgetPreferences({ ...widgetPreferences, listFontColor }), disable: true },
            { name: "Chat Bubble Color", variable: widgetPreferences.chatBubbleColor, method: (chatBubbleColor: string) => setWidgetPreferences({ ...widgetPreferences, chatBubbleColor }), disable: true },
            { name: "Chat Bubble Font Color", variable: widgetPreferences.chatFontColor, method: (chatFontColor: string) => setWidgetPreferences({ ...widgetPreferences, chatFontColor }), disable: true },
        ];
    };

    return (
        <>
            {preCheckErrors.length > 0 && <AreasInNeedOfAttention preCheckErrors={preCheckErrors} />}
            <ChatDemoHeader />
            <Paper className={cls.paper}>
                <Align direction="center">
                    <SaveOrCancel size="large" onSave={saveWidgetPreferences} />
                </Align>
                <Grid container alignItems="center" justify="center">
                    <Grid item xs={3}>
                        <Typography align="center" gutterBottom variant="h4">
                            Landing Header
                        </Typography>
                        {widgetPreferences && <HeaderEditor setEditorState={(landingHeader: string) => setWidgetPreferences({ ...widgetPreferences, landingHeader })} initialData={widgetPreferences.landingHeader} />}
                    </Grid>
                    <Grid item xs={4}>
                        {apiKey && <PalavyrDemoWidget preCheckErrors={preCheckErrors} apiKey={apiKey} iframeRefreshed={iframeRefreshed} />}
                    </Grid>
                    <Grid item xs={3}>
                        <Typography align="center" gutterBottom variant="h4">
                            Chat Header
                        </Typography>
                        {widgetPreferences && <HeaderEditor setEditorState={(chatHeader: string) => setWidgetPreferences({ ...widgetPreferences, chatHeader })} initialData={widgetPreferences.chatHeader} />}
                    </Grid>
                </Grid>
                <Grid container justify="center">
                    <div>
                        <Align>
                            <Typography align="center" gutterBottom variant="h4">
                                Widget Font
                            </Typography>
                        </Align>
                        <Align>
                            {widgetPreferences && (
                                <CustomSelect
                                    option={widgetPreferences.fontFamily}
                                    options={supportedFonts}
                                    width="50%"
                                    align="left"
                                    onChange={(event) => {
                                        const newFont = event.target.value as string;
                                        setWidgetPreferences({ ...widgetPreferences, fontFamily: newFont });
                                    }}
                                />
                            )}
                        </Align>
                    </div>
                </Grid>
            </Paper>
            <Divider variant="fullWidth" />
            <Paper>
                <Align>
                    <Typography className={cls.colorstext} variant="h4">
                        Select your widget colors
                    </Typography>
                </Align>
                <Align direction="center">
                    <SaveOrCancel size="large" onSave={saveWidgetPreferences} />
                </Align>
                <Align>{widgetPreferences && <FakeWidgets {...widgetPreferences} />}</Align>
                <div className={cls.gridList}>
                    {widgetPreferences &&
                        colorPickers(widgetPreferences).map((picker: ColorPickerType, index: number) => {
                            return (
                                <div>
                                    <Typography align="center" variant="body1" className={cls.pickerTitle} gutterBottom>
                                        {picker.name}
                                    </Typography>
                                    <SpaceEvenly>{picker.variable && <ChromePicker width="95%" disableAlpha color={picker.variable} onChangeComplete={(color: { hex: React.SetStateAction<string> }) => picker.method(color.hex)} />}</SpaceEvenly>
                                </div>
                            );
                        })}
                </div>
            </Paper>
        </>
    );
};
