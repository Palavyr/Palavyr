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

type Props = {
    showTimeStamp: boolean;
    profileAvatar?: string;
};

const useStyles = makeStyles(theme => ({
    messageTube: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.chatBubbleColor,
        overflowY: "scroll",
        flexGrow: 1,
    }),
    messageTubeContainer: (prefs: WidgetPreferences) => ({
        height: "100%",
        minHeight: "100%",
        paddingTop: "4rem",
        backgroundColor: prefs.chatBubbleColor,
        paddingLeft: "0.8rem",
        paddingRight: "0.8rem",
    }),
}));

export const Messages = ({ profileAvatar, showTimeStamp }: Props) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles({ ...preferences });

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
        <div id="messages" className={cls.messageTubeContainer} ref={messageRef}>
            {messages?.map((message, index) => (
                <div className={cls.messageTube} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                    {getComponentToRender(message, showTimeStamp)}
                </div>
            ))}
            {typing && <Loader typing={typing} />}
        </div>
    );
};
