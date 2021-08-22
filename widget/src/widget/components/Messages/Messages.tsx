import React, { useContext, useEffect, useRef } from "react";
import { useSelector, useDispatch } from "react-redux";
import format from "date-fns/format";
import { Loader } from "./components/Loader/Loader";
import { WidgetPreferences, GlobalState } from "@Palavyr-Types";
import { _markAllMessagesRead, _setBadgeCount } from "store/actions/actions";
import { scrollToBottom } from "widget/utils/messages";
import { getComponentToRender } from "widget/componentRegistry/getComponentToRender";
import { BrandingStrip } from "common/BrandingStrip";
import { makeStyles } from "@material-ui/core";

import "./styles.scss";
import classNames from "classnames";
import { WidgetContext } from "widget/context/WidgetContext";

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
        // if (showChat && badgeCount) dispatch(_markAllMessagesRead());
        // else dispatch(_setBadgeCount(messages.filter(message => message.unread).length));
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [messages, badgeCount, showChat, typing]);

    // Can make the avatar only appear on the message with the current index. So
    // TODO: Fix this function or change to move the avatar to last message from response
    // const shouldRenderAvatar = (message: Message, index: number) => {
    //   const previousMessage = messages[index - 1];
    //   if (message.showAvatar && previousMessage.showAvatar) {
    //     dispatch(hideAvatar(index));
    //   }
    // }

    const cls = useStyles({ ...preferences });

    return (
        <>
            <div id="messages" className={classNames("rcw-messages-container", cls.messageTube)} ref={messageRef} style={{ paddingBottom: "2rem" }}>
                {messages?.map((message, index) => (
                    <div className={classNames("rcw-message", cls.messageTube)} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                        {/* {profileAvatar  && message.showAvatar && <img src={profileAvatar} className="rcw-avatar" alt="profile" />} */}
                        {/* <FaceIcon className={cls.face} /> */}
                        {getComponentToRender(message, showTimeStamp)}
                    </div>
                ))}
                {typing && <Loader typing={typing} />}
            </div>
            <BrandingStrip />
        </>
    );
};
