import React, { useEffect, useCallback, useState, useContext } from "react";
import { Typography, Card, makeStyles, Divider, Box, Tab, Tabs } from "@material-ui/core";
import { serverUrl, widgetUrl } from "@common/client/clientUtils";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";
import Styles from "./minimalStyles.png";
import Precheck from "./precheck.png";
import { LineSpacer } from "@common/components/typography/LineSpacer";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
}

function TabPanel(props: TabPanelProps) {
    const { children, value, index, ...other } = props;

    return (
        <div role="tabpanel" hidden={value !== index} id={`simple-tabpanel-${index}`} aria-labelledby={`simple-tab-${index}`} {...other}>
            {value === index && (
                <Box sx={{ p: 3 }}>
                    <PalavyrText>{children}</PalavyrText>
                </Box>
            )}
        </div>
    );
}

function a11yProps(index: number) {
    return {
        id: `simple-tab-${index}`,
        "aria-controls": `simple-tabpanel-${index}`,
    };
}

const useStyles = makeStyles(theme => ({
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
    const [value, setValue] = React.useState(0);
    const handleChange = (event: React.SyntheticEvent, newValue: number) => {
        setValue(newValue);
    };

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
            <Box sx={{ width: "100%" }}>
                <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
                    <Tabs value={value} onChange={handleChange} aria-label="basic tabs example" centered>
                        <Tab label="HTML" {...a11yProps(0)} />
                        <Tab label="Pre-Check" {...a11yProps(1)} />
                        <Tab label="Wix" {...a11yProps(2)} />
                    </Tabs>
                </Box>
                <TabPanel value={value} index={0}>
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
                </TabPanel>
                <TabPanel value={value} index={1}>
                    <div style={{ display: "flex", flexDirection: "row", justifyContent: "space-around" }}>
                        <Card className={cls.outerCard}>
                            <Typography gutterBottom variant="h5">
                                Check if your widget is enabled before loading (This feature is currently disabled during Alpha testing )
                            </Typography>
                            <Typography paragraph>
                                <p>
                                    You may wish to perform a precheck on your widget before you attempt to load it in your website. Doing so will help guard against accidental loading of your widget when it is
                                    in an incomplete state.
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
                    </div>
                </TabPanel>
                <TabPanel value={value} index={2}>
                    TODO
                    {/* <FontSelector widgetPreferences={widgetPreferences} setWidgetPreferences={setWidgetPreferences} /> */}
                </TabPanel>
            </Box>
        </>
    );
};
