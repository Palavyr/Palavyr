import { Drawer, makeStyles } from "@material-ui/core";
import { WidgetNodeResource, WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { WidgetLayout } from "@widgetcore/widget/WidgetLayout";
import React, { useContext } from "react";
import { useWidgetStyles } from "@widgetcore/widget/Widget";
import classNames from "classnames";
import PalavyrChatWidget from "palavyr-chat-widget";
import { PalavyrWidgetRepository } from "@api-client/PalavyrWidgetRepository";
import { ComponentRegistry } from "@widgetcore/componentRegistry/registry";
import { DashboardContext } from "@frontend/dashboard/layouts/DashboardContext";

import "@widgetcore/widget/widget.module.scss";
import { useAppContext } from "widget/hook";

const drawerWidth = 400;

const useStyles = makeStyles(theme => ({
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
        overflowY: "hidden",
    },
    drawerRoot: {
        borderLeft: "5px solid grey",
    },

    drawerPaper: {
        width: drawerWidth,
        backgroundColor: theme.palette.background.default,
        display: "flex",
        textAlign: "center",
        flexDirection: "column",
        justifyContent: "center",
    },
    // necessary for content to be below app bar
    toolbar: theme.mixins.toolbar,
    widget: {
        height: "100%",
        width: "100%",
    },
    drawerRight: { height: "100%", width: drawerWidth, backgroundColor: theme.palette.background.default },
}));

export interface DesignerWidgetDrawerProps {
    widgetPreferences: WidgetPreferences;
}

export const DesignerWidgetDrawer = ({ widgetPreferences }: DesignerWidgetDrawerProps) => {
    const cls = useStyles();
    const wcls = useWidgetStyles();
    const { repository } = useContext(DashboardContext);
    const { addNewBotMessage } = useAppContext();

    const initializer = async () => {
        const apiKey = await repository.Settings.Account.getApiKey();
        const client = new PalavyrWidgetRepository(apiKey);
        const render = (componentType: string, text: string, nodeId: string, nodeChildrenString: string) => {
            const node = { text, nodeId, nodeChildrenString } as WidgetNodeResource;
            const componentMaker = ComponentRegistry[componentType];
            addNewBotMessage(componentMaker({ node, nodeList: [], client, convoId: "test-123", designer: true }));
        };

        render("ProvideInfo", "You can use this display to customize your widget.", "1", "2");

        const node = { text: "Here are some example buttons", nodeId: "2", nodeChildrenString: "3,4" } as WidgetNodeResource;
        const nodeList = [
            { nodeId: "3", optionPath: "Yes" },
            { nodeId: "4", optionPath: "No" },
        ];
        const componentMaker = ComponentRegistry["MultipleChoiceAsPath"];
        addNewBotMessage(componentMaker({ node, nodeList, client, convoId: "test-123", designer: true }));

        render("Selection", "Here is your intent selector.", "5", "6");
        render("CollectDetails", "Here is your details collector.", "6", "7");
        render("ProvideInfoWithPdfLink", "Here is your pdf link.", "7", "8");
        render("ProvideInfo", "Thanks so much for using Palavyr!", "8", "9");
    };

    const DrawerWidget = (
        <div className={classNames(cls.widget, wcls.pwbox)}>
            {widgetPreferences && (
                <WidgetContext.Provider value={{ preferences: widgetPreferences, chatStarted: true, setChatStarted: () => null, setConvoId: () => null, convoId: "demo" }}>
                    <WidgetLayout initializer={initializer} />
                </WidgetContext.Provider>
            )}
        </div>
    );

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
            <PalavyrChatWidget className={cls.widget} startOpen fixedPosition={false} alternateContent={DrawerWidget} style={{ height: "90vh" }} />
        </Drawer>
    );
};
