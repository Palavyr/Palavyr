import React, { useEffect, useCallback, useState, useContext } from "react";
import { Typography, Card, makeStyles, Divider } from "@material-ui/core";
import { serverUrl, widgetUrl } from "@common/client/clientUtils";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";
import Styles from "./minimalStyles.png";
import Precheck from "./precheck.png";
import { LineSpacer } from "@common/components/typography/LineSpacer";

const useStyles = makeStyles((theme) => ({
    outerCard: {
        margin: "4rem",
        padding: "3rem",
        textAlign: "center",
    },
    img: {
        height: "300px",
        borderRadius: "15px",
    },
}));

export const GetWidget = () => {
    const { repository } = useContext(DashboardContext);
    const [apikey, setApiKey] = useState<string>("");
    const cls = useStyles();

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
            <HeaderStrip title="Get the Widget" subtitle="Use the following code snippets to integrate the widget into your side. We are currently working on a widget api, so stay tuned!" />
            <Card className={cls.outerCard}>
                <Typography gutterBottom variant="h5">
                    Add the configured widget to your website
                </Typography>
                <Typography gutterBottom paragraph>
                    To add the widget to your website, simply paste the following code into your website's html and apply custom styling:
                </Typography>
                {apikey !== "" && (
                    <Typography style={{ cursor: "pointer" }} onClick={() => window.open(`${widgetUrl}/widget?key=${apikey}`, "_blank")} component="pre" paragraph>
                        <strong>
                            &lt;iframe src="{widgetUrl}/widget?key={apikey}" /&gt;
                        </strong>
                    </Typography>
                )}
                <Typography gutterBottom>Minimal Style Recommendation</Typography>
                <ZoomImage alt="min styles" imgSrc={Styles} className={cls.img} />
            </Card>
            <Card className={cls.outerCard}>
                <Typography gutterBottom variant="h5">
                    Check if your widget is enabled before loading (This feature is currently disabled during Alpha testing )
                </Typography>
                <Typography paragraph>
                    <p>
                        You may wish to perform a precheck on your widget before you attempt to load it in your website. Doing so will help guard against accidental loading of your widget when it is in an
                        incomplete state.
                    </p>
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
                <LineSpacer numLines={2} />
                <ZoomImage alt="precheck" imgSrc={Precheck} className={cls.img} />
            </Card>
        </>
    );
};
