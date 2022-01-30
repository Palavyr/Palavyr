import React, { memo, useContext, useEffect, useMemo, useRef } from "react";
import format from "date-fns/format";
import { Loader } from "./components/Loader/Loader";
import { BotMessageData, UserMessageData, WidgetPreferences } from "@Palavyr-Types";
import { scrollToBottom } from "@widgetcore/utils/messages";
import { makeStyles } from "@material-ui/core";

import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { MessageWrapper } from "../../BotResponse/utils/MessageWrapper";

import "@widgetcore/widget/widget.module.scss";
import { useWidgetStyles } from "@widgetcore/widget/Widget";
import classNames from "classnames";

export interface MessageProps {
    showTimeStamp: boolean;
    profileAvatar?: string;
}

const useStyles = makeStyles(theme => ({
    message: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.chatBubbleColor,
        overflowY: "hidden",
        marginTop: ".6rem",
        marginBottom: ".6rem"
    }),
    messageTubeContainer: (prefs: WidgetPreferences) => ({
        paddingTop: "2rem",
        backgroundColor: prefs.chatBubbleColor,
        paddingLeft: "0.8rem",
        paddingRight: "0.8rem",
    }),
}));

export const MessageTypes = {
    BOT: "bot",
    USER: "user",
};

export const getComponentToRender = (message: UserMessageData | BotMessageData, showTimeStamp: boolean) => {
    if (message.type === MessageTypes.BOT) {
        const MessageComponent = message.component as BotMessageData["component"];
        return (
            <MessageWrapper>
                <MessageComponent {...message.props} />
            </MessageWrapper>
        );
    } else if (message.type === MessageTypes.USER) {
        const MessageComponent = message.component;
        return <MessageComponent message={message} showTimeStamp={showTimeStamp} />;
    } else {
        throw new Error("Unknown message type");
    }
};

const MessageSlice = memo(({ message, showTimeStamp }: { message: UserMessageData | BotMessageData; showTimeStamp: boolean }) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles({ ...preferences });
    return <div className={cls.message}>{getComponentToRender(message, showTimeStamp)}</div>;
});

export const Messages = ({ profileAvatar, showTimeStamp }: MessageProps) => {
    const { preferences, context } = useContext(WidgetContext);
    const cls = useStyles({ ...preferences });
    const wcls = useWidgetStyles();

    useEffect(() => {
        document.body.setAttribute("style", `overflow: "hidden"`);
    }, []);

    const messageRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        scrollToBottom(messageRef.current);
    }, [context.messages, context.loading]);

    useEffect(() => {
        if (context.messages.length > 0) {
            scrollToBottom(messageRef.current);
        }
    }, [context.messages, context.loading]);

    return (
        <div id="messages" className={classNames(wcls.pwrow, wcls.pcontent, cls.messageTubeContainer)} ref={messageRef}>
            {context.messages.length > 0 && context.messages.map((message, index) => <MessageSlice message={message} showTimeStamp={showTimeStamp} key={`${index}-${format(message.timestamp, "hh:mm")}`} />)}
            {context.loading && <Loader />}
            <div style={{ height: "6rem" }} />
        </div>
    );
};
