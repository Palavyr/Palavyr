import { Drawer, makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import React from "react";
import { WidgetContext } from "./fakeWidgetNew/components/context/WidgetContext";
import { SmoothWidget } from "./fakeWidgetNew/components/widgets/SmoothWidget";

const drawerWidth = 440;

const useStyles = makeStyles(theme => ({
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
    },
    drawerRoot: { height: "100%" },

    drawerPaper: {
        width: drawerWidth,
        height: "100%",
    },
    // necessary for content to be below app bar
    toolbar: theme.mixins.toolbar,
    widget: {
        height: "100%",
        width: "400px",
        display: "flex",
        flexDirection: "column",
        padding: "1rem",
    },
    drawerRight: { height: "100%" },
}));

export interface DesignerWidgetDrawerProps {
    widgetPreferences: WidgetPreferences;
}

export const DesignerWidgetDrawer = ({ widgetPreferences }: DesignerWidgetDrawerProps) => {
    const cls = useStyles();
    return (
        <Drawer
            className={cls.drawer}
            variant="permanent"
            classes={{
                paperAnchorDockedRight: cls.drawerRight,
                // root: cls.drawerRoot,
                // paper: cls.drawerPaper,
            }}
            anchor="right"
        >
            <div className={cls.toolbar} />
            <div className={cls.widget}>
                {widgetPreferences && (
                    <WidgetContext.Provider value={{ preferences: widgetPreferences, chatStarted: true, setChatStarted: () => null, setConvoId: () => null, convoId: "demo" }}>
                        <SmoothWidget />
                    </WidgetContext.Provider>
                )}
            </div>
        </Drawer>
    );
};
