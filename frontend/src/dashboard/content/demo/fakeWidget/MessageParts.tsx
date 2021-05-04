import { Box, makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import classNames from "classnames";
import React from "react";
import { FakeMessage } from "./fakeMessages";

type StyleProps = {
    backgroundColor: string;
    color: string;
};

const makeChatBodyColor = (props: StyleProps) => {
    let chatBodyStyles = {
        backgroundColor: "#F4F4F4",
        color: "white",
        paddingLeft: "0.2rem",
        paddingRight: "0.2rem",
        paddingTop: "0.1rem",
        borderRadius: "10px",
        maxWidth: "85%",
    };

    if (props.backgroundColor) {
        chatBodyStyles = { ...chatBodyStyles, backgroundColor: props.backgroundColor };
    }
    if (props.color) {
        chatBodyStyles = { ...chatBodyStyles, color: props.color };
    }
    return chatBodyStyles;
};

const useStyles = makeStyles((theme) => ({
    messageText: (props: StyleProps) => makeChatBodyColor(props),
    layout: {
        textAlign: "left",
        overflowX: "scroll",
    },
}));

interface IWrapMessages {
    customPreferences: WidgetPreferences;
    children: React.ReactNode;
}

const MessageWrapper = ({ customPreferences, children }: IWrapMessages) => {
    const cls = useStyles({ color: customPreferences.chatFontColor, backgroundColor: customPreferences.chatBubbleColor });
    return <Box className={classNames(cls.messageText, cls.layout)}>{children}</Box>;
};

export const getComponentToRender = (message: FakeMessage, customPreferences: WidgetPreferences) => {
    const ComponentToRender = message.component;
    if (message.sender === "response") {
        return (
            <MessageWrapper customPreferences={customPreferences}>
                <ComponentToRender />
            </MessageWrapper>
        );
    }
    return <ComponentToRender />;
};
