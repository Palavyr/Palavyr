import { MESSAGE_BOX_SCROLL_DURATION } from "../constants";

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
        if (total < scrollStart + scroll + 100) {
            // I ADDED 100 HERE TO TRY AND MAKE THIS SCROLL TO THE BOTTOM WITH AUTOCOMPLETE
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

export function scrollToTop(messagesDiv: HTMLDivElement | null) {
    if (!messagesDiv) return;
    const scrollTop = messagesDiv.scrollTop;
    if (scrollTop) scrollWithSlowMotion(messagesDiv, scrollTop, -scrollTop);
}
