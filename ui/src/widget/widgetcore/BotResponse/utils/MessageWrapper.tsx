import * as React from "react";
import { Box, makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { WidgetContext } from "@widgetcore/context/WidgetContext";

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
        merginRight: "0.4rem",
        marginTop: "0.1rem",
        borderRadius: "10px",
    },
    timeStamp: {
        fontSize: "9px",
        marginTop: "0px",
        borderTop: "1px solid grey",
        float: "left",
    },
}));

export interface IWrapMessages {
    children: React.ReactNode;
}

export const MessageWrapper = ({ children }: IWrapMessages) => {
    const { preferences } = React.useContext(WidgetContext);
    const cls = useStyles({ color: preferences.chatFontColor, backgroundColor: preferences.chatBubbleColor });
    return (
        <div className={cls.wrapper}>
            <Box className={classNames(cls.messageText, cls.layout)}>{children}</Box>
        </div>
    );
};
