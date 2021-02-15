import React, { useEffect, useRef } from "react";
import { useSelector, useDispatch } from "react-redux";
import format from "date-fns/format";

import { scrollToBottom } from "../../../../../../utils/messages";
import { Message, Link, CustomCompMessage, GlobalState } from "../../../../../../store/types";
import { _setBadgeCount, _markAllMessagesRead } from "src/widgetCore/store/actions";

import Loader from "./components/Loader";
import "./styles.scss";
import { WidgetPreferences } from "src/types";
import { MessageWrapper } from "src/componentRegistry/common";

type Props = {
    showTimeStamp: boolean;
    profileAvatar?: string;
    customPreferences: WidgetPreferences;
};

function Messages({ profileAvatar, showTimeStamp, customPreferences }: Props) {
    const dispatch = useDispatch();
    const { messages, typing, showChat, badgeCount } = useSelector((state: GlobalState) => ({
        messages: state.messages.messages,
        badgeCount: state.messages.badgeCount,
        typing: state.behavior.messageLoader,
        showChat: state.behavior.showChat
    }));

    const messageRef = useRef<HTMLDivElement | null>(null);
    useEffect(() => {
        // @ts-ignore
        scrollToBottom(messageRef.current);
        if (showChat && badgeCount) dispatch(_markAllMessagesRead());
        else dispatch(_setBadgeCount(messages.filter(message => message.unread).length));
    }, [messages, badgeCount, showChat]);

    const getComponentToRender = (message: Message | Link | CustomCompMessage) => {
        const ComponentToRender = message.component;
        if (message.type === "component") {
            return <MessageWrapper customPreferences={customPreferences}><ComponentToRender {...message.props} /></MessageWrapper>;
        }
        return <ComponentToRender message={message} showTimeStamp={showTimeStamp} />;
    };

    // TODO: Fix this function or change to move the avatar to last message from response
    // const shouldRenderAvatar = (message: Message, index: number) => {
    //   const previousMessage = messages[index - 1];
    //   if (message.showAvatar && previousMessage.showAvatar) {
    //     dispatch(hideAvatar(index));
    //   }
    // }

    return (
        <div id="messages" className="rcw-messages-container" ref={messageRef} style={{paddingBottom: "2rem"}}>
            {messages?.map((message, index) => (
                <div className="rcw-message" key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                    {profileAvatar/* && message.showAvatar*/ && <img src={profileAvatar} className="rcw-avatar" alt="profile" />}
                    {getComponentToRender(message)}
                </div>
            ))}
            <Loader typing={typing} />
        </div>
    );
}

export default Messages;
