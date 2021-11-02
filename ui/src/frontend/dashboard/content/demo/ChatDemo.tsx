import { PreCheckError, WidgetPreferences } from "@Palavyr-Types";
import React, { useState, useCallback, useEffect } from "react";
import { Grid, Paper, makeStyles } from "@material-ui/core";
import { AreasInNeedOfAttention } from "./AreasInNeedOfAttention";
import { ChatDemoHeader } from "./ChatDemoHeader";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { PalavyrDemoWidget } from "./DemoWidget";

import { useContext } from "react";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Align } from "@common/positioning/Align";

const useStyles = makeStyles(theme => ({
    paper: {
        padding: theme.spacing(5),
        marginTop: theme.spacing(1),
        background: "none",
        border: "none",
        boxShadow: "none",
    },
    button: {
        marginBottom: "1rem",
    },
    reloadButton: {
        position: "fixed",
        top: "50%",
        right: "2rem",
    },
}));

export const ChatDemoPage = () => {
    const { repository, setViewName } = useContext(DashboardContext);
    setViewName("Widget Demo");
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
        <Paper className={cls.paper}>
            <AreasInNeedOfAttention preCheckErrors={preCheckErrors} />
            <ChatDemoHeader />
            <Grid container alignItems="center" justify="center">
                <Grid item xs={4}>
                    {apiKey && <PalavyrDemoWidget preCheckErrors={preCheckErrors} apiKey={apiKey} iframeRefreshed={iframeRefreshed} />}
                </Grid>
            </Grid>
            <div className={cls.reloadButton}>
                <SinglePurposeButton classes={cls.button} variant="outlined" color="primary" buttonText="Reload" onClick={() => window.location.reload()} />
            </div>
        </Paper>
    );
};