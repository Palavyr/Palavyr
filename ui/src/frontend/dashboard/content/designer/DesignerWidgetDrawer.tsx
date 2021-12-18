import { Drawer, makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { WidgetLayout } from "@widgetcore/widget/WidgetLayout";
import React from "react";
import { Provider } from "react-redux";
import { PalavyrWidgetStore } from "widget/store/store";
import { useWidgetStyles } from "@widgetcore/widget/Widget";
import classNames from "classnames";
import '@widgetcore/widget/widget.module.scss';

const drawerWidth = 400;

const useStyles = makeStyles(theme => ({
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
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
        height: "100%",
        overflowY: "hidden",
    },
    drawerRight: { height: "100%", backgroundColor: theme.palette.background.default },
}));

export interface DesignerWidgetDrawerProps {
    widgetPreferences: WidgetPreferences;
}

export const DesignerWidgetDrawer = ({ widgetPreferences }: DesignerWidgetDrawerProps) => {
    const cls = useStyles();
    const wcls = useWidgetStyles();
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
            <Provider store={PalavyrWidgetStore}>
                <div className={classNames(cls.widget, wcls.pwbox)}>
                    {widgetPreferences && (
                        <WidgetContext.Provider value={{ preferences: widgetPreferences, chatStarted: true, setChatStarted: () => null, setConvoId: () => null, convoId: "demo" }}>
                            <WidgetLayout />
                        </WidgetContext.Provider>
                    )}
                </div>
            </Provider>
        </Drawer>
    );
};
