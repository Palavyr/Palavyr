import React, { useContext, useEffect, useRef } from "react";
import { useSelector } from "react-redux";
import format from "date-fns/format";
import { Loader } from "./components/Loader/Loader";
import { WidgetPreferences, GlobalState } from "@Palavyr-Types";
import { _markAllMessagesRead, _setBadgeCount } from "@store-actions";
import { scrollToBottom } from "@widgetcore/utils/messages";
import { getComponentToRender } from "@widgetcore/BotResponse/utils/getComponentToRender";
import { makeStyles } from "@material-ui/core";

import { WidgetContext } from "@widgetcore/context/WidgetContext";

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
    }),
    // messageTubeContainer: (prefs: WidgetPreferences) => ({
    //     overflowY: "scroll",
    //     // minHeight: "100%",
    //     paddingTop: "2rem",
    //     backgroundColor: prefs.chatBubbleColor,
    //     paddingLeft: "0.8rem",
    //     paddingRight: "0.8rem",
    //     display: "flex",
    //     flexDirection: "column",
    //     // flex: "1 1 auto"
    // }),
}));

export const Messages = ({ profileAvatar, showTimeStamp }: MessageProps) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles({ ...preferences });
    const wcls = useWidgetStyles();

    const { messages, typing } = useSelector((state: GlobalState) => ({
        messages: state.messagesReducer.messages,
        typing: state.behaviorReducer.messageLoader,
    }));
    useEffect(() => {
        document.body.setAttribute("style", `overflow: "hidden"`);
    }, []);

    const messageRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        scrollToBottom(messageRef.current);
    }, [messages, typing]);

    return (
        <div id="messages" className={classNames(wcls.pwrow, wcls.pcontent)} ref={messageRef}>
            {messages?.map((message, index) => (
                <div className={cls.message} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                    {getComponentToRender(message, showTimeStamp)}
                </div>
            ))}
            {typing && <Loader typing={typing} />}
        </div>
    );
};
