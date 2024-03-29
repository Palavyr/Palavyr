import { widgetUrl } from "@common/client/clientUtils";
import { makeStyles, Typography, useTheme } from "@material-ui/core";
import { Alert } from "@material-ui/lab";
import { PreCheckErrorResource } from "@Palavyr-Types";
import { Align } from "@common/positioning/Align";
import React from "react";
import { IFrame } from "./IFrame";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    paper: {
        display: "flex",
        justifyContent: "center",
        padding: theme.spacing(5),
        marginTop: theme.spacing(3),
    },
}));

export interface DemoWidgetProps {
    preCheckErrors: PreCheckErrorResource[];
    apiKey: string;
    iframeRefreshed: boolean;
}

export const PalavyrDemoWidget = ({ preCheckErrors, apiKey, iframeRefreshed }: DemoWidgetProps) => {
    const theme = useTheme();
    return (
        <div>
            {preCheckErrors.length > 0 && (
                <Alert severity="error" style={{ borderLeft: `3px solid ${theme.palette.error.dark}`, borderTopLeftRadius: "0px", borderBottomLeftRadius: "0px", marginTop: "1rem", marginBottom: "1.5rem" }}>
                    <Typography gutterBottom>The demo will load once you've fully assembled each of your intents!</Typography>
                </Alert>
            )}
            <Align>
                <IFrame widgetUrl={widgetUrl} apiKey={apiKey} iframeRefreshed={iframeRefreshed} preCheckErrors={preCheckErrors} />
            </Align>
        </div>
    );
};
