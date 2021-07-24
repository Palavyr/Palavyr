import React, { useEffect, useCallback, useState, useContext } from "react";
import { Typography, Card, makeStyles, Divider } from "@material-ui/core";
import { serverUrl, widgetUrl } from "@api-client/clientUtils";
import { Align } from "dashboard/layouts/positioning/Align";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

const useStyles = makeStyles((theme) => ({
    outerCard: {
        margin: "4rem",
        padding: "3rem",
        textAlign: "center",
    },
}));

export const GetWidget = () => {
    const { repository } = useContext(DashboardContext);
    const [apikey, setApiKey] = useState<string>("");
    const classes = useStyles();

    const loadApiKey = useCallback(async () => {
        const key = await repository.Settings.Account.getApiKey();
        console.log(`ApiKey: ${key}`);
        setApiKey(key);
    }, []);

    useEffect(() => {
        loadApiKey();
    }, []);

    return (
        <>
            <AreaConfigurationHeader title="Get the Widget" subtitle="Use the following code snippets to integrate the widget into your side. We are currently working on a widget api, so stay tuned!" />
            <Card className={classes.outerCard}>
                <Typography gutterBottom variant="h5">
                    Add the configured widget to your website
                </Typography>
                <Typography gutterBottom paragraph>
                    To add the widget to your website, simply paste the following code into your website's html and apply custom styling:
                </Typography>
                {apikey !== "" && (
                    <Typography style={{cursor: "pointer"}} onClick={() => window.open(`${widgetUrl}/widget?key=${apikey}`, "_blank")} component="pre" paragraph>
                        <strong>
                            &lt;iframe src="{widgetUrl}/widget?key={apikey}" /&gt;
                        </strong>
                    </Typography>
                )}
                <Typography>
                    <p>Minimal Style Recommendation</p>
                    <pre>height: "500px", width: "380px",</pre>
                </Typography>
            </Card>
            <Card className={classes.outerCard}>
                <Typography gutterBottom variant="h5">
                    Check if your widget is enabled before loading (This feature is currently disabled during Alpha testing )
                </Typography>
                <Typography paragraph>
                    <p>You may wish to perform a precheck on your widget before you attempt to load it in your website. Doing so will help guard against accidental loading of your widget when it is in an incomplete state.</p>
                    <p>To do this, send a request to the following url:</p>
                    <strong>
                        {serverUrl}/api/widget/widget-precheck?key={apikey}
                    </strong>
                </Typography>
                <Typography>
                    This request must be made with the following custom header:
                    <p>
                        <pre>action=widgetAccess</pre>
                    </p>
                </Typography>
                <Divider />
                <Align direction="center">
                    <Typography align="left">
                        <p>
                            <strong>Example pre-check request</strong>
                        </p>
                        <pre>
                            {`
                    <script>
                        var iframeId = "id-of-your-iframe-container-element";
                        var url = ${serverUrl}/api/widget/widget-precheck?key=${apikey}
                        fetch(
                            url,
                            {
                                method: "GET",
                                headers: { action: "widgetAccess" },
                            }
                        )
                        .then((response) => response.json())
                        .then((result) => {
                            if (!result.isReady) document.getElementById(iframeId).innerHTML = "";
                        })
                        .catch((error) => console.error("Error:", error);
                    </script>
                        `}
                        </pre>
                    </Typography>
                </Align>
            </Card>
        </>
    );
};
