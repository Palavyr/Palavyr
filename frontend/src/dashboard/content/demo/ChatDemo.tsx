import { PreCheckError, WidgetPreferences } from "@Palavyr-Types";
import React, { useState, useCallback, useEffect } from "react";
import { Grid, Paper, Typography, makeStyles, Divider } from "@material-ui/core";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { CustomSelect } from "../responseConfiguration/response/tables/dynamicTable/CustomSelect";
import { AreasInNeedOfAttention } from "./AreasInNeedOfAttention";
import { ChatDemoHeader } from "./ChatDemoHeader";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { PalavyrDemoWidget } from "./DemoWidget";
import { Align } from "dashboard/layouts/positioning/Align";

import { useContext } from "react";

const useStyles = makeStyles(theme => ({
    paper: {
        padding: theme.spacing(5),
        marginTop: theme.spacing(3),
    },
}));

export const ChatDemoPage = () => {
    const { repository } = useContext(DashboardContext);

    const [preCheckErrors, setPreCheckErrors] = useState<PreCheckError[]>([]);
    const [apiKey, setApiKey] = useState<string>("");
    const [iframeRefreshed, reloadIframe] = useState<boolean>(false);
    const [widgetPreferences, setWidgetPreferences] = useState<WidgetPreferences>();

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
        const key = await repository.Settings.Account.getApiKey();
        setApiKey(key);

        const currentWidgetPreferences = await repository.WidgetDemo.GetWidetPreferences();
        setWidgetPreferences(currentWidgetPreferences);
    }, []);

    useEffect(() => {
        loadDemoWidget();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <>
            {preCheckErrors.length > 0 && <AreasInNeedOfAttention preCheckErrors={preCheckErrors} />}
            <ChatDemoHeader />
            <Paper className={cls.paper}>
                <Grid container alignItems="center" justify="center">
                    <Grid item xs={4}>
                        {apiKey && <PalavyrDemoWidget preCheckErrors={preCheckErrors} apiKey={apiKey} iframeRefreshed={iframeRefreshed} />}
                    </Grid>
                </Grid>
            </Paper>
        </>
    );
};
