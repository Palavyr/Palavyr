import React, { useEffect, useRef } from "react";
import { useSelector, useDispatch } from "react-redux";
import format from "date-fns/format";
import "./styles.scss";

import { Loader } from "./components/Loader/Loader";
import { WidgetPreferences, GlobalState } from "@Palavyr-Types";
import { _markAllMessagesRead, _setBadgeCount } from "store/actions/actions";
import { scrollToBottom } from "widget/utils/messages";
import { getComponentToRender } from "componentRegistry/getComponentToRender";

type Props = {
    showTimeStamp: boolean;
    profileAvatar?: string;
    preferences: WidgetPreferences;
};

export const Messages = ({ preferences, profileAvatar, showTimeStamp }: Props) => {
    const dispatch = useDispatch();
    const { messages, typing, showChat, badgeCount } = useSelector((state: GlobalState) => ({
        messages: state.messagesReducer.messages,
        badgeCount: state.messagesReducer.badgeCount,
        typing: state.behaviorReducer.messageLoader,
        showChat: state.behaviorReducer.showChat,
    }));

    const messageRef = useRef<HTMLDivElement | null>(null);
    useEffect(() => {
        scrollToBottom(messageRef.current);
        if (showChat && badgeCount) dispatch(_markAllMessagesRead());
        else dispatch(_setBadgeCount(messages.filter(message => message.unread).length));
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [messages, badgeCount, showChat]);

    // TODO: Fix this function or change to move the avatar to last message from response
    // const shouldRenderAvatar = (message: Message, index: number) => {
    //   const previousMessage = messages[index - 1];
    //   if (message.showAvatar && previousMessage.showAvatar) {
    //     dispatch(hideAvatar(index));
    //   }
    // }

    return (
        <div id="messages" className="rcw-messages-container" ref={messageRef} style={{ paddingBottom: "2rem" }}>
            {messages?.map((message, index) => (
                <div className="rcw-message" key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                    {profileAvatar /* && message.showAvatar*/ && <img src={profileAvatar} className="rcw-avatar" alt="profile" />}
                    {getComponentToRender(message, preferences, showTimeStamp)}
                </div>
            ))}
            <Loader typing={typing} />
        </div>
    );
};
