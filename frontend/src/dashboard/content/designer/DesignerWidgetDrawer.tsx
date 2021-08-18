import { Drawer, makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import React from "react";
import { FakeWidgets } from "./fakeWidget/FakeWidgets";

const drawerWidth = 440;

const useStyles = makeStyles(theme => ({
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
    },
    drawerRoot: {},

    drawerPaper: {
        width: drawerWidth,
    },
    // necessary for content to be below app bar
    toolbar: theme.mixins.toolbar,
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
                root: cls.drawerRoot,
                paper: cls.drawerPaper,
            }}
            anchor="right"
        >
            <div className={cls.toolbar} />
            <FakeWidgets {...widgetPreferences} />
        </Drawer>
    );
};
