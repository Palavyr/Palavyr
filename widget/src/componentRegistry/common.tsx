import * as React from "react";
import { Box, makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "src/types";
import classNames from "classnames";

type StyleProps = {
    backgroundColor: string;
    color: string;
};

const makeChatBodyColor = (props: StyleProps) => {
    let chatBodyStyles = {
        backgroundColor: "green",
        color: "white",
        borderRadius: "10px",
        padding: "15px",
        maxWidth: "70%",
    };

    if (props.backgroundColor) {
        chatBodyStyles = { ...chatBodyStyles, backgroundColor: props.backgroundColor };
    }
    if (props.color) {
        chatBodyStyles = { ...chatBodyStyles, color: props.color };
    }
    return chatBodyStyles;
};

const useStyles = makeStyles(theme => ({
    messageText: (props: StyleProps) => makeChatBodyColor(props),
    layout: {
        textAlign: "left",
        overflowX: "scroll",

    },
}));

export interface IWrapMessages {
    customPreferences: WidgetPreferences;
    children: React.ReactNode;
}

export const MessageWrapper = ({ customPreferences, children }: IWrapMessages) => {
    const cls = useStyles({ color: customPreferences.chatFontColor, backgroundColor: customPreferences.chatBubbleColor });
    return <Box boxShadow={1} className={classNames(cls.messageText, cls.layout)}>{children}</Box>;
};
