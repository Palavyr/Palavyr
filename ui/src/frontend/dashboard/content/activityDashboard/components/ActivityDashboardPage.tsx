import React, { useEffect } from "react";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { makeStyles, Grid } from "@material-ui/core";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { DailyEnquiriesWeekly } from "../DailyEnquiriesWeekly";
import { EnquiryActivity } from "../EnquiryActivity";

import { Theme } from "@material-ui/core/styles";
import AppBar from "@material-ui/core/AppBar";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";
import { IntentActivityCards } from "../IntentActivityCards.tsx/IntentActivityCards";

interface TabPanelProps {
    children?: React.ReactNode;
    index: any;
    value: any;
}

function TabPanel(props: TabPanelProps) {
    const { children, value, index, ...other } = props;

    return (
        <div role="tabpanel" hidden={value !== index} id={`scrollable-auto-tabpanel-${index}`} aria-labelledby={`scrollable-auto-tab-${index}`} {...other}>
            {value === index && (
                <Box p={3}>
                    <Typography>{children}</Typography>
                </Box>
            )}
        </div>
    );
}

function a11yProps(index: any) {
    return {
        id: `scrollable-auto-tab-${index}`,
        "aria-controls": `scrollable-auto-tabpanel-${index}`,
    };
}

const useStyles = makeStyles((theme: Theme) => ({
    root: {
        width: "100%",
        height: "50%",
        backgroundColor: theme.palette.background.default,
    },
}));

export const ActivityDashboardPage = () => {
    const { setViewName } = React.useContext(DashboardContext);

    useEffect(() => {
        setViewName("Data Dashboard");
    }, []);

    const cls = useStyles();
    const [value, setValue] = React.useState(0);

    const handleChange = (event: React.ChangeEvent<{}>, newValue: number) => {
        setValue(newValue);
    };

    return (
        <div className={cls.root}>
            <HeaderStrip
                divider
                title="Widget Activity Dashboard"
                subtitle="Review the activity of your chatbot! This page is early release, but we've made a couple plots availble as a sneak peak for you!"
            />
            <AppBar style={{ boxShadow: "none" }} position="static" color="default">
                <Tabs value={value} onChange={handleChange} indicatorColor="primary" textColor="primary" variant="scrollable" scrollButtons="auto" aria-label="scrollable auto tabs example">
                    <Tab label="Intent Insights" {...a11yProps(0)} />
                    <Tab label="Enquiry Activity" {...a11yProps(1)} />
                    <Tab label="Weekly Activity" {...a11yProps(2)} />
                </Tabs>
            </AppBar>

                <ActivityTabPanel value={value} index={0}>
                    <IntentActivityCards />
                </ActivityTabPanel>
                <ActivityTabPanel value={value} index={1}>
                    <EnquiryActivity />
                </ActivityTabPanel>
                <ActivityTabPanel value={value} index={2}>
                    <DailyEnquiriesWeekly />
                </ActivityTabPanel>

        </div>
    );
};

const ActivityTabPanel = ({ children, value, index }) => {
    return (
        <TabPanel value={value} index={index}>
            <div style={{ height: "500px" }}>{children}</div>
        </TabPanel>
    );
};
