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
    messageRef: React.MutableRefObject<HTMLDivElement | null>;
};

const useStyles = makeStyles(theme => ({
    face: {
        height: "4rem",
    },
    messageTube: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.chatBubbleColor,
        height: "100%",
        overflowY: "scroll",
        paddingTop: "15px",
        paddingBottom: "2rem",
        // "-webkit-overflow-scrolling": "touch",
    }),
}));

export const Messages = ({ profileAvatar, showTimeStamp, messageRef }: Props) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles({ ...preferences });

    const { messages, typing } = useSelector((state: GlobalState) => ({
        messages: state.messagesReducer.messages,
        typing: state.behaviorReducer.messageLoader,
    }));

    // const messageRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        scrollToBottom(messageRef.current);
    }, [messages, typing]);

    return (
        <div id="messages" className={cls.messageTube} ref={messageRef}>
            {messages?.map((message, index) => (
                <div className={cls.messageTube} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                    {getComponentToRender(message, showTimeStamp)}
                </div>
            ))}
            {typing && <Loader typing={typing} />}
        </div>
    );
};
