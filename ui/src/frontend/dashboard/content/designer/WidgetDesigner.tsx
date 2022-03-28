import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { Box, makeStyles, Tab, Tabs } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { Align } from "@common/positioning/Align";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { ColorSelectors } from "./colors/ColorSelectors";
import { DesignerWidgetDrawer } from "./DesignerWidgetDrawer";
import { FontSelector } from "./fonts/FontSelector";
import { DesignChatHeader } from "./headers/ChatHeader";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { InitializeFonts } from "./fonts/Initializer";

const drawerWidth = 440;

const useStyles = makeStyles(theme => ({
    paper: {
        padding: theme.spacing(5),
        marginTop: theme.spacing(3),
        height: "100%",
    },
    drawerRoot: {},
    root: {
        display: "flex",
        height: "100%",
    },
    appBar: {
        width: `calc(100% - ${drawerWidth}px)`,
        marginRight: drawerWidth,
    },
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
    },
    drawerPaper: {
        width: drawerWidth,
    },
    // necessary for content to be below app bar
    toolbar: theme.mixins.toolbar,
    content: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.default,
        padding: theme.spacing(3),
    },
}));

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

export const WidgetDesignerPage = () => {
    const [value, setValue] = React.useState(0);

    const handleChange = (event: React.SyntheticEvent, newValue: number) => {
        setValue(newValue);
    };

    const { repository, setViewName } = useContext(DashboardContext);
    setViewName("Widget Designer");

    const cls = useStyles();
    const [widgetPreferences, setWidgetPreferences] = useState<WidgetPreferences>();

    const saveWidgetPreferences = async () => {
        if (widgetPreferences) {
            const updatedPreferences = await repository.WidgetDemo.SaveWidgetPreferences(widgetPreferences);
            setWidgetPreferences(updatedPreferences);
            return true;
        } else {
            return false;
        }
    };

    const loadDemoWidget = useCallback(async () => {
        const currentWidgetPreferences = await repository.WidgetDemo.GetWidetPreferences();
        setWidgetPreferences(currentWidgetPreferences);
    }, []);

    useEffect(() => {
        loadDemoWidget();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        // Initialize FontManager object
        if (widgetPreferences) {
            InitializeFonts(widgetPreferences);
        }
    }, [widgetPreferences]);

    return (
        <>
            {widgetPreferences && (
                <div className={cls.root}>
                    <div className={cls.content}>
                        <div style={{ position: "fixed" }}>
                            <Align direction="flex-end">
                                <SaveOrCancel zIndex={3000} size="large" onSave={saveWidgetPreferences} />
                            </Align>
                        </div>
                    </div>
                    <Box sx={{ width: "100%" }}>
                        <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
                            <Tabs value={value} onChange={handleChange} aria-label="basic tabs example" centered>
                                <Tab label="Colors" {...a11yProps(0)} />
                                <Tab label="Header" {...a11yProps(1)} />
                                <Tab label="Fonts" {...a11yProps(2)} />
                            </Tabs>
                        </Box>
                        <TabPanel value={value} index={0}>
                            <ColorSelectors widgetPreferences={widgetPreferences} setWidgetPreferences={setWidgetPreferences} />
                        </TabPanel>
                        <TabPanel value={value} index={1}>
                            <div style={{ display: "flex", flexDirection: "row", justifyContent: "space-around" }}>
                                <DesignChatHeader widgetPreferences={widgetPreferences} setWidgetPreferences={setWidgetPreferences} />
                            </div>
                        </TabPanel>
                        <TabPanel value={value} index={2}>
                            <FontSelector widgetPreferences={widgetPreferences} setWidgetPreferences={setWidgetPreferences} />
                        </TabPanel>
                    </Box>
                    <DesignerWidgetDrawer widgetPreferences={widgetPreferences} />
                </div>
            )}
        </>
    );
};
