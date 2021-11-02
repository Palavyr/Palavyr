import {
    ADD_NEW_USER_MESSAGE,
    MessagesState,
    ADD_NEW_RESPONSE_MESSAGE,
    ADD_NEW_LINK_SNIPPET,
    ADD_COMPONENT_MESSAGE,
    DROP_MESSAGES,
    HIDE_AVATAR,
    DELETE_MESSAGES,
    SET_BADGE_COUNT,
    MARK_ALL_READ,
    MessagesActions,
} from "@Palavyr-Types";
import { MESSAGE_SENDER } from "@widgetcore/constants";
import { createNewMessage, createLinkSnippet, createComponentMessage } from "@widgetcore/utils/messages";
import { createReducer } from "./createReducer";

export type MessageState = {
    messages: Array<React.ReactNode>;
    badgeCount: number;
};

const initialState: MessageState = {
    messages: [],
    badgeCount: 0,
};

const reducer = {
    [ADD_NEW_USER_MESSAGE]: (state: MessagesState, { text, id }) => ({ ...state, messages: [...state.messages, createNewMessage(text, MESSAGE_SENDER.CLIENT, id)] }),

    [ADD_NEW_RESPONSE_MESSAGE]: (state: MessagesState, { text, id }) => ({
        ...state,
        messages: [...state.messages, createNewMessage(text, MESSAGE_SENDER.RESPONSE, id)],
        badgeCount: state.badgeCount + 1,
    }),

    [ADD_NEW_LINK_SNIPPET]: (state: MessagesState, { link, id }) => ({ ...state, messages: [...state.messages, createLinkSnippet(link, id)] }),

    [ADD_COMPONENT_MESSAGE]: (state: MessagesState, { component, props, showAvatar, id }) => ({ ...state, messages: [...state.messages, createComponentMessage(component, props, showAvatar, id)] }),

    [DROP_MESSAGES]: (state: MessagesState) => ({ ...state, messages: [] }),

    [HIDE_AVATAR]: (state: MessagesState, { index }) => (state.messages[index].showAvatar = false),

    [DELETE_MESSAGES]: (state: MessagesState, { count, id }) => ({
        ...state,
        messages: id ? state.messages.filter(message => message.customId !== id) : state.messages.splice(state.messages.length - 1, count),
    }),

    [SET_BADGE_COUNT]: (state: MessagesState, { count }) => ({ ...state, badgeCount: count }),

    [MARK_ALL_READ]: (state: MessagesState) => ({ ...state, messages: state.messages.map(message => ({ ...message, unread: false })), badgeCount: 0 }),
};

export const messagesReducer = (state: MessageState = initialState, action: MessagesActions) => createReducer<MessageState, MessagesActions>(reducer, state, action);