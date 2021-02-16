import { ElementType } from "react";

import * as actionsTypes from "./types";
import { LinkParams, ImageState, ContextProperties, KeyValue, DynamicResponse } from "../types";


export function _openUserDetails(): actionsTypes.OpenUserDetails {
    return {
        type: actionsTypes.OPEN_USER_DETAILS
    }
}

export function _closeUserDetails(): actionsTypes.CloseUserDetails {
    return {
        type: actionsTypes.CLOSE_USER_DETAILS
    }
}

export function _toggleUserDetails(): actionsTypes.ToggleUserDetails {
    return {
        type: actionsTypes.TOGGLE_USER_DETAILS
    }
}

export function _toggleChat(): actionsTypes.ToggleChat {
    return {
        type: actionsTypes.TOGGLE_CHAT,
    };
}

export function _toggleInputDisabled(): actionsTypes.ToggleInputDisabled {
    return {
        type: actionsTypes.TOGGLE_INPUT_DISABLED,
    };
}

export function _disableInput(): actionsTypes.InputDisabled {
    return {
        type: actionsTypes.DISABLE_INPUT,
    };
}

export function _enableInput(): actionsTypes.InputEnabled {
    return {
        type: actionsTypes.ENABLE_INPUT,
    };
}


export function _addUserMessage(text: string, id?: string): actionsTypes.AddUserMessage {
    return {
        type: actionsTypes.ADD_NEW_USER_MESSAGE,
        text,
        id,
    };
}

export function _addResponseMessage(text: string, id?: string): actionsTypes.AddResponseMessage {
    return {
        type: actionsTypes.ADD_NEW_RESPONSE_MESSAGE,
        text,
        id,
    };
}

export function _toggleMsgLoader(): actionsTypes.ToggleMsgLoader {
    return {
        type: actionsTypes.TOGGLE_MESSAGE_LOADER,
    };
}

export function _addLinkSnippet(link: LinkParams, id?: string): actionsTypes.AddLinkSnippet {
    return {
        type: actionsTypes.ADD_NEW_LINK_SNIPPET,
        link,
        id,
    };
}

export function _renderCustomComponent(component: ElementType, props: any, showAvatar: boolean, id?: string): actionsTypes.RenderCustomComponent {
    return {
        type: actionsTypes.ADD_COMPONENT_MESSAGE,
        component,
        props,
        showAvatar,
        id,
    };
}

export function _dropMessages(): actionsTypes.DropMessages {
    return {
        type: actionsTypes.DROP_MESSAGES,
    };
}

export function _hideAvatar(index: number): actionsTypes.HideAvatar {
    return {
        type: actionsTypes.HIDE_AVATAR,
        index,
    };
}

export function _setQuickButtons(buttons: Array<{ label: string; value: string | number }>): actionsTypes.SetQuickButtons {
    return {
        type: actionsTypes.SET_QUICK_BUTTONS,
        buttons,
    };
}

export function _deleteMessages(count: number, id?: string): actionsTypes.DeleteMessages {
    return {
        type: actionsTypes.DELETE_MESSAGES,
        count,
        id,
    };
}

export function _setBadgeCount(count: number): actionsTypes.SetBadgeCount {
    return {
        type: actionsTypes.SET_BADGE_COUNT,
        count,
    };
}

export function _markAllMessagesRead(): actionsTypes.MarkAllMessagesRead {
    return {
        type: actionsTypes.MARK_ALL_READ,
    };
}

export function _openFullscreenPreview(payload: ImageState): actionsTypes.FullscreenPreviewActions {
    return {
        type: actionsTypes.OPEN_FULLSCREEN_PREVIEW,
        payload,
    };
}

export function _closeFullscreenPreview(): actionsTypes.FullscreenPreviewActions {
    return {
        type: actionsTypes.CLOSE_FULLSCREEN_PREVIEW,
    };
}

export function _setContextProperties(contextProperties: ContextProperties): actionsTypes.ContextPropertyActions {
    return {
        type: actionsTypes.SET_CONTEXT_PROPERTIES,
        contextProperties,
    };
}

export function _setNameContext(name: string): actionsTypes.ContextPropertyActions {
    return {
        type: actionsTypes.SET_NAME_CONTEXT,
        name,
    };
}

export function _setPhoneContext(phoneNumber: string): actionsTypes.ContextPropertyActions {
    return {
        type: actionsTypes.SET_PHONE_CONTEXT,
        phoneNumber,
    };
}

export function _setEmailAddressContext(emailAddress: string): actionsTypes.ContextPropertyActions {
    return {
        type: actionsTypes.SET_EMAILADDRESS_CONTEXT,
        emailAddress,
    };
}

export function _setRegionContext(region: string): actionsTypes.ContextPropertyActions {
    return {
        type: actionsTypes.SET_REGION_CONTEXT,
        region,
    };
}

export function _addKeyValue(keyValue: KeyValue): actionsTypes.ContextPropertyActions {
    return {
        type: actionsTypes.SET_KEYVALUE_CONTEXT,
        keyValue,
    };
}

export function _addDynamicResponse(dynamicResponse: DynamicResponse): actionsTypes.ContextPropertyActions {
    return {
        type: actionsTypes.SET_DYNAMICRESPONSE_CONTEXT,
        dynamicResponse,
    };
}
