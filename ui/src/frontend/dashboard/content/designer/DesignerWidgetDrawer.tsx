import { Drawer, makeStyles, useTheme } from "@material-ui/core";
import { WidgetNodeResource, WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { WidgetLayout } from "@widgetcore/widget/WidgetLayout";
import React, { useEffect } from "react";
import { useWidgetStyles } from "@widgetcore/widget/Widget";
import classNames from "classnames";
import PalavyrChatWidget from "palavyr-chat-widget";
import { PalavyrWidgetRepository } from "@api-client/PalavyrWidgetRepository";
import { ComponentRegistry } from "@widgetcore/componentRegistry/registry";

import "@widgetcore/widget/widget.module.scss";
import { IAppContext, useAppContext } from "widget/hook";
import { useWindowDimensions } from "@common/hooks/useWindowDimensions";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { MessageTypes } from "@widgetcore/components/Messages/Messages";
import { CSS_LINKER_and_NODE_TYPE } from "@widgetcore/BotResponse/utils/responseAction";
import scrollToTop from "@common/utils/scrollToTop";

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

const render = (componentType: string, text: string, nodeId: string, nodeChildrenString: string, context: IAppContext, client: PalavyrWidgetRepository) => {
    const node = { text, nodeId, nodeChildrenString } as WidgetNodeResource;
    const component = ComponentRegistry[componentType]({ node, nodeList: [], client, convoId: "test-123", designer: true });
    const message = {
        type: MessageTypes.BOT,
        component,
        props: {},
        sender: CSS_LINKER_and_NODE_TYPE.BOT,
        timestamp: new Date(),
        showAvatar: true,
        customId: "",
        unread: true,
        nodeType: "",
        specialId: "",
    };
    context.addNewBotMessage(message);
};
const initializer = async (context: IAppContext, repository: PalavyrRepository) => {
    const apiKey = await repository.Settings.Account.getApiKey();
    const client = new PalavyrWidgetRepository(apiKey);

    render("ProvideInfo", "You can use this display to customize your widget.", "1", "2", context, client);

    // Special Snowflake button code
    const node = { text: "Here are some example buttons", nodeId: "2", nodeChildrenString: "3,4" } as WidgetNodeResource;
    const nodeList = [
        { nodeId: "3", optionPath: "Yes" },
        { nodeId: "4", optionPath: "No" },
    ];
    const component = ComponentRegistry["MultipleChoiceAsPath"]({ node, nodeList, client, convoId: "test-123", designer: true });
    const m = {
        type: MessageTypes.BOT,
        component,
        props: {},
        sender: CSS_LINKER_and_NODE_TYPE.BOT,
        timestamp: new Date(),
        showAvatar: true,
        customId: "",
        unread: true,
        nodeType: "",
        specialId: "",
    };
    context.addNewBotMessage(m);

    render("Selection", "Here is your intent selector.", "5", "6", context, client);
    render("CollectDetails", "Here is your details collector.", "6", "7", context, client);
    render("ProvideInfoWithPdfLink", "Here is your pdf link.", "7", "8", context, client);
    render("ProvideInfo", "Thanks so much for using Palavyr!", "8", "9", context, client);
    render("TakeNumber", "Give us a number.", "9", "10", context, client);
};

export const DesignerWidgetDrawer = ({ widgetPreferences }: DesignerWidgetDrawerProps) => {
    const cls = useStyles();
    const wcls = useWidgetStyles();
    const theme = useTheme();
    const context = useAppContext();

    const DrawerWidget = (
        <div className={classNames(cls.widget, wcls.pwbox)}>
            {widgetPreferences && (
                <WidgetContext.Provider value={{ preferences: widgetPreferences, chatStarted: true, setChatStarted: () => null, setConvoId: () => null, convoId: "demo", context }}>
                    <WidgetLayout initializer={initializer} />
                </WidgetContext.Provider>
            )}
        </div>
    );

    const { height: windowHeight } = useWindowDimensions();

    let chatHeight: number;
    if (theme.mixins.toolbar.minHeight) {
        chatHeight = windowHeight - parseInt(theme.mixins.toolbar.minHeight.toString());
    } else if (theme.mixins.toolbar.height) {
        chatHeight = windowHeight - parseInt(theme.mixins.toolbar.height.toString());
    } else {
        chatHeight = windowHeight;
    }

    useEffect(() => {
        scrollToTop();
    }, []);

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
            <PalavyrChatWidget IframeProps={{ className: cls.widget }} startOpen fixedPosition={false} alternateContent={DrawerWidget} containerStyles={{ height: `${chatHeight}px` }} />
        </Drawer>
    );
};
