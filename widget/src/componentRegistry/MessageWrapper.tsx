import * as React from "react";
import { Box, makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { WidgetPreferences } from "@Palavyr-Types";
import format from "date-fns/format";

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
        // overflowX: "scroll",
    },
    wrapper: {
        marginLeft: "0.2rem",
        merginRight: "0.2rem",
        marginTop: "0.1rem",
        borderRadius: "10px",
        maxWidth: "85%",
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
            <span style={{ float: "left" }} className="rcw-timestamp">
                {format(new Date(), "hh:mm")}
            </span>
        </div>
    );
};
