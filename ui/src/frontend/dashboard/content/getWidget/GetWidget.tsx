import React, { useEffect, useCallback, useState, useContext } from "react";
import { Typography, makeStyles, Box, Tab, Tabs } from "@material-ui/core";
import { widgetUrl } from "@common/client/clientUtils";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";
import Styles from "./minimalStyles.png";
import SiteBuilders from "./SiteBuilders.png";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { Align } from "@common/positioning/Align";

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
    outerdiv: {
        padding: "3rem",
        textAlign: "center",
    },
    img: {
        height: "300px",
        borderRadius: "15px",
        marginTop: "3rem",
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
        const key = await repository.Settings.Account.GetApiKey();
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
                        <Tab label="React" {...a11yProps(1)} />
                        <Tab label="Point and Click Site Builders" {...a11yProps(2)} />
                        {/* <Tab label="Pre-Check" {...a11yProps(3)} /> */}
                    </Tabs>
                </Box>
                <TabPanel value={value} index={0}>
                    <div className={cls.outerdiv}>
                        <Typography gutterBottom variant="h5">
                            Add the widget to HTML
                        </Typography>
                        <Typography gutterBottom paragraph>
                            To add the widget to your website's HTML, simply paste the following code into your website's html and apply custom styling:
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
                    </div>
                </TabPanel>
                <TabPanel value={value} index={1}>
                    <div className={cls.outerdiv}>
                        <Align verticalCenter direction="center" orientation="column">
                            <Typography gutterBottom variant="h5">
                                Use the all new React Chat Widget
                            </Typography>
                            <PalavyrText gutterBottom>
                                Examples for how to place your widget using React can be found on{" "}
                                <a target="_blank" href="https://www.npmjs.com/package/palavyr-chat-widget">
                                    NPM
                                </a>{" "}
                                and the open source{" "}
                                <a target="_blank" href="https://github.com/Palavyr/palavyr-chat-widget">
                                    Palavyr Chat Widget
                                </a>{" "}
                                project.
                            </PalavyrText>
                            <PalavyrText>
                                For a working demo of the widget, visit the{" "}
                                <a target="_blank" href="https://palavyr.github.io/palavyr-chat-widget/">
                                    Demo Webpage
                                </a>
                                .
                            </PalavyrText>
                        </Align>
                    </div>
                </TabPanel>
                <TabPanel value={value} index={2}>
                    <div className={cls.outerdiv}>
                        <Align verticalCenter direction="center" orientation="column">
                            <Typography gutterBottom variant="h5" style={{}}>
                                Embed the widget in your point-and-click site builder website
                            </Typography>
                            <PalavyrText gutterBottom style={{ maxWidth: "75%", marginBottom: "1rem" }}>
                                Point and click site builders are websites that allow you to create a websites quickly directly through your web browsers. These are websites such as sitebuilder.com, wix.com,
                                godaddy.com, etc.
                            </PalavyrText>
                            <PalavyrText>To embed a widget in your website, you can provide your widget's url to your site builder's tool for embedding iframes or links. For example:</PalavyrText>

                            <div className={cls.img}>
                                <Typography gutterBottom>Click to zoom</Typography>
                                <ZoomImage alt="Site Builders" imgSrc={SiteBuilders} />
                            </div>
                        </Align>
                    </div>
                </TabPanel>
            </Box>
        </>
    );
};
