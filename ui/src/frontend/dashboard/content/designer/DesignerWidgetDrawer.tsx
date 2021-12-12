import { Drawer, makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { SmoothWidget } from "@widgetcore/widgets/SmoothWidget";
import React from "react";
import { Provider } from "react-redux";
import { PalavyrWidgetStore } from "widget/store/store";

const drawerWidth = 400;

const useStyles = makeStyles(theme => ({
    drawer: {
        width: drawerWidth,
        // flexShrink: 0,
        overflowY: "hidden",
    },
    drawerRoot: { borderLeft: "5px solid grey" },

    drawerPaper: {
        width: drawerWidth,
        backgroundColor: theme.palette.background.default,
    },
    // necessary for content to be below app bar
    toolbar: theme.mixins.toolbar,
    widget: {
        padding: "1rem",
        width: "100%",
        height: "90%",
        background: "none",
    },
    drawerRight: { height: "100%", backgroundColor: theme.palette.background.default },
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
                root: cls.drawerRoot,
                paper: cls.drawerPaper,
            }}
            anchor="right"
        >
            <div className={cls.toolbar} />
            <div className={cls.widget}>
                {widgetPreferences && (
                    <Provider store={PalavyrWidgetStore}>
                        <WidgetContext.Provider value={{ preferences: widgetPreferences, chatStarted: true, setChatStarted: () => null, setConvoId: () => null, convoId: "demo" }}>
                            <SmoothWidget fakeApiKey="123" />
                        </WidgetContext.Provider>
                    </Provider>
                )}
            </div>
        </Drawer>
    );
};
