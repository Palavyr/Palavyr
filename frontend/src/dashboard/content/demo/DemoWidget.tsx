import { widgetUrl } from "@api-client/clientUtils";
import { makeStyles, Paper, Typography } from "@material-ui/core";
import { Alert } from "@material-ui/lab";
import { IncompleteArea, PreCheckError } from "@Palavyr-Types";
import { Align } from "dashboard/layouts/positioning/AlignCenter";
import React from "react";
import { IFrame } from "./IFrame";

const useStyles = makeStyles((theme) => ({
    paper: {
        display: "flex",
        justifyContent: "center",
        padding: theme.spacing(5),
        marginTop: theme.spacing(3),
    },
}));

export interface DemoWidgetProps {
    preCheckErrors: PreCheckError[];
    apiKey: string;
    iframeRefreshed: boolean;
}

export const PalavyrDemoWidget = ({ preCheckErrors, apiKey, iframeRefreshed }: DemoWidgetProps) => {
    const cls = useStyles();

    return (
        <div>
            {preCheckErrors.length > 0 && (
                <Alert severity="error" style={{ marginTop: "1rem" }}>
                    <Typography gutterBottom>The Demo will load once you've fully assembled each of your areas!</Typography>
                </Alert>
            )}
            <Align>
                <IFrame widgetUrl={widgetUrl} apiKey={apiKey} iframeRefreshed={iframeRefreshed} preCheckErrors={preCheckErrors} />
            </Align>
        </div>
    );
};
