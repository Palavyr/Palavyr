import React, { useEffect, useCallback, useState } from "react";
import { Typography, Card, makeStyles } from "@material-ui/core";
import { ApiClient } from "@api-client/Client";
import { serverUrl, widgetUrl } from "@api-client/clientUtils";

const useStyles = makeStyles((theme) => ({
    outerCard: {
        margin: "4rem",
        padding: "3rem",
        textAlign: "center",
    },
}));

export const GetWidget = () => {
    const client = new ApiClient();
    const [apikey, setApiKey] = useState<string>("");
    const classes = useStyles();

    const loadApiKey = useCallback(async () => {
        const { data: key } = await client.Settings.Account.getApiKey();
        console.log(`ApiKey: ${key}`);
        setApiKey(key);
    }, []);

    useEffect(() => {
        loadApiKey();
    }, []);

    return (
        <>
            <Card className={classes.outerCard}>
                <Typography gutterBottom={true} variant="h4">
                    Add the configured widget to your website
                </Typography>

                <Typography paragraph>To add the widget to your website, simply paste the following code into your website's html and apply custom styling:</Typography>
                {apikey !== "" && (
                    <Typography component="pre" paragraph>
                        <strong>&lt;iframe src="{widgetUrl}/widget?key={apikey}" /&gt;</strong>
                    </Typography>
                )}
                <Typography>
                    <p>Minimal Style Recommendation</p>
                    <pre>
                    height: "500px",
                    width: "380px",
                    </pre>
                </Typography>
            </Card>
            <Card className={classes.outerCard}>
                <Typography gutterBottom={true} variant="h4">
                    Check if your widget is enabled before loading
                </Typography>
                <Typography component="pre" paragraph>
                <p>To do this, send a request to the following url:</p>
                    <strong>{serverUrl}/api/widget/widget-active-state?key={apikey}</strong>
                </Typography>
                <Typography>
                    This request must be made with the following custom header:
                    <p>
                        <strong>action=widgetAccess</strong>
                    </p>
                </Typography>
            </Card>
        </>
    );
};
