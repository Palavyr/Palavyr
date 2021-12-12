import React, { useContext, useEffect, useRef } from "react";
import { useSelector } from "react-redux";
import format from "date-fns/format";
import { Loader } from "./components/Loader/Loader";
import { WidgetPreferences, GlobalState } from "@Palavyr-Types";
import { _markAllMessagesRead, _setBadgeCount } from "@store-actions";
import { scrollToBottom } from "@widgetcore/utils/messages";
import { getComponentToRender } from "@widgetcore/BotResponse/utils/getComponentToRender";
import { makeStyles } from "@material-ui/core";

import "./styles.scss";
import classNames from "classnames";
import { WidgetContext } from "@widgetcore/context/WidgetContext";

type Props = {
    showTimeStamp: boolean;
    profileAvatar?: string;
};

const useStyles = makeStyles(theme => ({
    face: {
        height: "4rem",
    },
    messageTube: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.chatBubbleColor,
    }),
}));

export const Messages = ({ profileAvatar, showTimeStamp }: Props) => {
    const { preferences } = useContext(WidgetContext);

    const { messages, typing, showChat } = useSelector((state: GlobalState) => ({
        messages: state.messagesReducer.messages,
        typing: state.behaviorReducer.messageLoader,
        showChat: state.behaviorReducer.showChat,
    }));

    useEffect(() => {
        scrollToBottom(messageRef.current);
    }, [messages, showChat, typing]);

    const messageRef = useRef<HTMLDivElement | null>(null);

    const cls = useStyles({ ...preferences });

    return (
        <>
            <div id="messages" className={classNames("rcw-messages-container", cls.messageTube)} ref={messageRef} style={{ paddingBottom: "2rem" }}>
                {messages?.map((message, index) => (
                    <div className={classNames("rcw-message", cls.messageTube)} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                        {getComponentToRender(message, showTimeStamp)}
                    </div>
                ))}
                {typing && <Loader typing={typing} />}
            </div>
        </>
    );
};
