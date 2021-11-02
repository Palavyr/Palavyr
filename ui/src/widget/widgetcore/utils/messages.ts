import { ElementType } from "react";
import { CustomCompMessage, IMessage, Link, LinkParams } from "@Palavyr-Types";
import { Message } from "@widgetcore/components/Messages/components/Message/Message";
import { Snippet } from "@widgetcore/components/Messages/components/Snippet/Snippet";

import { MESSAGES_TYPES, MESSAGE_SENDER, MESSAGE_BOX_SCROLL_DURATION } from "../constants";

export const createNewMessage = (text: string, sender: string, id?: string): IMessage => {
    return {
        type: MESSAGES_TYPES.TEXT,
        component: Message,
        text,
        sender,
        timestamp: new Date(),
        showAvatar: sender === MESSAGE_SENDER.RESPONSE,
        customId: id,
        unread: sender === MESSAGE_SENDER.RESPONSE,
    };
};

export const createLinkSnippet = (link: LinkParams, id?: string): Link => {
    return {
        type: MESSAGES_TYPES.SNIPPET.LINK,
        component: Snippet,
        title: link.title,
        link: link.link,
        target: link.target || "_blank",
        sender: MESSAGE_SENDER.RESPONSE,
        timestamp: new Date(),
        showAvatar: true,
        customId: id,
        unread: true,
    };
};

export const createComponentMessage = (component: ElementType, props: any, showAvatar: boolean, id?: string): CustomCompMessage => {
    return {
        type: MESSAGES_TYPES.CUSTOM_COMPONENT,
        component,
        props,
        sender: MESSAGE_SENDER.RESPONSE,
        timestamp: new Date(),
        showAvatar,
        customId: id,
        unread: true,
    };
};

export const sinEaseOut = (timestamp: any, begining: any, change: any, duration: any) => {
    return change * ((timestamp = timestamp / duration - 1) * timestamp * timestamp + 1) + begining;
};

/**
 *
 * @param {*} target scroll target
 * @param {*} scrollStart
 * @param {*} scroll scroll distance
 */
export const scrollWithSlowMotion = (target: any, scrollStart: any, scroll: number) => {
    const raf = window?.requestAnimationFrame;
    let start = 0;
    const step = timestamp => {
        if (!start) {
            start = timestamp;
        }
        let stepScroll = sinEaseOut(timestamp - start, 0, scroll, MESSAGE_BOX_SCROLL_DURATION);
        let total = scrollStart + stepScroll;
        target.scrollTop = total;
        if (total < scrollStart + scroll + 100) { // I ADDED 100 HERE TO TRY AND MAKE THIS SCROLL TO THE BOTTOM WITH AUTOCOMPLETE
            raf(step);
        }
    };
    raf(step);
};

export function scrollToBottom(messagesDiv: HTMLDivElement | null) {
    if (!messagesDiv) return;
    const screenHeight = messagesDiv.clientHeight;
    const scrollTop = messagesDiv.scrollTop;
    const scrollOffset = messagesDiv.scrollHeight - (scrollTop + screenHeight);
    if (scrollOffset) scrollWithSlowMotion(messagesDiv, scrollTop, scrollOffset);
}
