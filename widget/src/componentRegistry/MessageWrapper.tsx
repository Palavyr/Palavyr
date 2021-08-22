import * as React from "react";
import { Box, makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { WidgetPreferences } from "@Palavyr-Types";

type StyleProps = {
    backgroundColor: string;
    color: string;
};

const makeChatBodyColor = (props: StyleProps) => {
    let chatBodyStyles = {
        backgroundColor: "#F4F4F4",
        color: "white",
        borderRadius: "10px",
        width: "100%",
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
    },
    wrapper: {
        // marginLeft: "0.2rem",
        merginRight: "0.4rem",
        marginTop: "0.1rem",
        borderRadius: "10px",
        // maxWidth: "85%",
        // minWidth: "45%",
    },
    timeStamp: {
        fontSize: "9px",
        marginTop: "0px",
        borderTop: "1px solid black",
        float: "left",
    },
}));

export interface IWrapMessages {
    customPreferences: WidgetPreferences;
    children: React.ReactNode;
}

export const MessageWrapper = ({ customPreferences, children }: IWrapMessages) => {
    const cls = useStyles({ color: customPreferences.chatFontColor, backgroundColor: customPreferences.chatBubbleColor });
    return (
        <div className={cls.wrapper}>
            <Box className={classNames(cls.messageText, cls.layout)}>{children}</Box>
        </div>
    );
};
