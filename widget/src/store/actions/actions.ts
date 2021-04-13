import {
    LinkParams,
    ImageState,
    ContextProperties,
    KeyValue,
    DynamicResponses,
    AddLinkSnippet,
    AddResponseMessage,
    AddUserMessage,
    ADD_COMPONENT_MESSAGE,
    ADD_NEW_LINK_SNIPPET,
    ADD_NEW_RESPONSE_MESSAGE,
    ADD_NEW_USER_MESSAGE,
    CloseUserDetails,
    CLOSE_FULLSCREEN_PREVIEW,
    CLOSE_USER_DETAILS,
    ContextPropertyActions,
    DeleteMessages,
    DELETE_MESSAGES,
    DISABLE_INPUT,
    DropMessages,
    DROP_MESSAGES,
    ENABLE_INPUT,
    FullscreenPreviewActions,
    HideAvatar,
    HIDE_AVATAR,
    InputDisabled,
    InputEnabled,
    MarkAllMessagesRead,
    MARK_ALL_READ,
    OpenUserDetails,
    OPEN_FULLSCREEN_PREVIEW,
    OPEN_USER_DETAILS,
    RenderCustomComponent,
    SetBadgeCount,
    SET_BADGE_COUNT,
    SET_CONTEXT_PROPERTIES,
    SET_DYNAMICRESPONSES_CONTEXT,
    SET_EMAILADDRESS_CONTEXT,
    SET_KEYVALUE_CONTEXT,
    SET_NAME_CONTEXT,
    SET_NUM_INDIVIDUALS_CONTEXT,
    SET_PHONE_CONTEXT,
    SET_REGION_CONTEXT,
    ToggleChat,
    ToggleInputDisabled,
    ToggleMsgLoader,
    ToggleUserDetails,
    TOGGLE_CHAT,
    TOGGLE_INPUT_DISABLED,
    TOGGLE_MESSAGE_LOADER,
    TOGGLE_USER_DETAILS,
} from "@Palavyr-Types";

import { ElementType } from "react";

export const _openUserDetails = (): OpenUserDetails => {
    return {
        type: OPEN_USER_DETAILS,
    };
}

export const _closeUserDetails = (): CloseUserDetails => {
    return {
        type: CLOSE_USER_DETAILS,
    };
}

export const _toggleUserDetails = (): ToggleUserDetails => {
    return {
        type: TOGGLE_USER_DETAILS,
    };
}

export const _toggleChat = (): ToggleChat => {
    return {
        type: TOGGLE_CHAT,
    };
}

export const _toggleInputDisabled = (): ToggleInputDisabled => {
    return {
        type: TOGGLE_INPUT_DISABLED,
    };
}

export const _disableInput = (): InputDisabled => {
    return {
        type: DISABLE_INPUT,
    };
}

export const _enableInput = (): InputEnabled => {
    return {
        type: ENABLE_INPUT,
    };
}

export const _addUserMessage = (text: string, id?: string): AddUserMessage => {
    return {
        type: ADD_NEW_USER_MESSAGE,
        text,
        id,
    };
}

export const _addResponseMessage = (text: string, id?: string): AddResponseMessage => {
    return {
        type: ADD_NEW_RESPONSE_MESSAGE,
        text,
        id,
    };
}

export const _toggleMsgLoader = (): ToggleMsgLoader => {
    return {
        type: TOGGLE_MESSAGE_LOADER,
    };
}

export const _addLinkSnippet = (link: LinkParams, id?: string): AddLinkSnippet => {
    return {
        type: ADD_NEW_LINK_SNIPPET,
        link,
        id,
    };
}

export const _renderCustomComponent = (component: ElementType, props: any, showAvatar: boolean, id?: string): RenderCustomComponent => {
    return {
        type: ADD_COMPONENT_MESSAGE,
        component,
        props,
        showAvatar,
        id,
    };
}

export const _dropMessages = (): DropMessages => {
    return {
        type: DROP_MESSAGES,
    };
}

export const _hideAvatar = (index: number): HideAvatar => {
    return {
        type: HIDE_AVATAR,
        index,
    };
}

export const _deleteMessages = (count: number, id?: string): DeleteMessages => {
    return {
        type: DELETE_MESSAGES,
        count,
        id,
    };
}

export const _setBadgeCount = (count: number): SetBadgeCount => {
    return {
        type: SET_BADGE_COUNT,
        count,
    };
}

export const _markAllMessagesRead = (): MarkAllMessagesRead => {
    return {
        type: MARK_ALL_READ,
    };
}

export const _openFullscreenPreview = (payload: ImageState): FullscreenPreviewActions => {
    return {
        type: OPEN_FULLSCREEN_PREVIEW,
        payload,
    };
}

export const _closeFullscreenPreview = (): FullscreenPreviewActions => {
    return {
        type: CLOSE_FULLSCREEN_PREVIEW,
    };
}

export const _setContextProperties = (contextProperties: ContextProperties): ContextPropertyActions => {
    return {
        type: SET_CONTEXT_PROPERTIES,
        contextProperties,
    };
}

export const _setNameContext = (name: string): ContextPropertyActions => {
    return {
        type: SET_NAME_CONTEXT,
        name,
    };
}

export const _setPhoneContext = (phoneNumber: string): ContextPropertyActions => {
    return {
        type: SET_PHONE_CONTEXT,
        phoneNumber,
    };
}

export const _setEmailAddressContext = (emailAddress: string): ContextPropertyActions => {
    return {
        type: SET_EMAILADDRESS_CONTEXT,
        emailAddress,
    };
}

export const _setRegionContext = (region: string): ContextPropertyActions => {
    return {
        type: SET_REGION_CONTEXT,
        region,
    };
}

export const _setNumIndividualsContext = (numIndividuals: number) => {
    return {
        type: SET_NUM_INDIVIDUALS_CONTEXT,
        numIndividuals,
    };
}

export const _addKeyValue = (keyValue: KeyValue): ContextPropertyActions => {
    return {
        type: SET_KEYVALUE_CONTEXT,
        keyValue,
    };
}

export const _setDynamicResponses = (dynamicResponseObject: DynamicResponses): ContextPropertyActions => {
    return {
        type: SET_DYNAMICRESPONSES_CONTEXT,
        dynamicResponseObject,
    };
}
