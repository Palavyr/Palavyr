import { ElementType } from "react";

import { LinkParams, FullscreenPreviewState, ContextProperties, KeyValue, DynamicResponse, DynamicResponses } from "../types";


export const OPEN_USER_DETAILS = "BEHAVIOR/OPEN_USER_DETAILS";
export const CLOSE_USER_DETAILS = "BEHAVIOR/CLOSE_USER_DETAILS";
export const TOGGLE_USER_DETAILS = "BEHAVIOR/TOGGLE_USER_DETAILS"
export const TOGGLE_CHAT = "BEHAVIOR/TOGGLE_CHAT";
export const TOGGLE_INPUT_DISABLED = "BEHAVIOR/TOGGLE_INPUT_DISABLED";
export const DISABLE_INPUT = "BEHAVIOR/INPUT_DISABLED";
export const ENABLE_INPUT = "BEHAVIOR/INPUT_ENABLED";
export const TOGGLE_MESSAGE_LOADER = "BEHAVIOR/TOGGLE_MSG_LOADER";
export const SET_BADGE_COUNT = "BEHAVIOR/SET_BADGE_COUNT";
export const ADD_NEW_USER_MESSAGE = "MESSAGES/ADD_NEW_USER_MESSAGE";
export const ADD_NEW_RESPONSE_MESSAGE = "MESSAGES/ADD_NEW_RESPONSE_MESSAGE";
export const ADD_NEW_LINK_SNIPPET = "MESSAGES/ADD_NEW_LINK_SNIPPET";
export const ADD_COMPONENT_MESSAGE = "MESSAGES/ADD_COMPONENT_MESSAGE";
export const DROP_MESSAGES = "MESSAGES/DROP_MESSAGES";
export const HIDE_AVATAR = "MESSAGES/HIDE_AVATAR";
export const DELETE_MESSAGES = "MESSAGES/DELETE_MESSAGES";
export const MARK_ALL_READ = "MESSAGES/MARK_ALL_READ";
export const SET_QUICK_BUTTONS = "SET_QUICK_BUTTONS";
export const OPEN_FULLSCREEN_PREVIEW = "FULLSCREEN/OPEN_PREVIEW";
export const CLOSE_FULLSCREEN_PREVIEW = "FULLSCREEN/CLOSE_PREVIEW";
export const SET_CONTEXT_PROPERTIES = "CONTEXT_PROPERTIES/SET";
export const GET_CONTEXT_PROPERTIES = "CONTEXT_PROPERTIES/GET";

export const SET_NUM_INDIVIDUALS_CONTEXT = "CONTEXT_PROPERTIES/SET_NUM_INDIVIDUALS"
export const SET_NAME_CONTEXT = "CONTEXT_PROPERTIES/SET_NAME"
export const SET_PHONE_CONTEXT = "CONTEXT_PROPERTIES/SET_PHONE"
export const SET_EMAILADDRESS_CONTEXT = "CONTEXT_PROPERTIES/SET_EMAILADDRESS"
export const SET_REGION_CONTEXT = "CONTEXT_PROPERTIES/SET_REGION"
export const SET_KEYVALUE_CONTEXT = "CONTEXT_PROPERTIES/SET_KEYVALUE"
export const SET_DYNAMICRESPONSE_CONTEXT = "CONTEXT_PROPERTIES/SET_DYNAMICRESPONSE"
export const SET_DYNAMICRESPONSES_CONTEXT = "CONTEXT_PROPERTIES/SET_DYNAMICRESPONSES"

export interface OpenUserDetails {
    type: typeof OPEN_USER_DETAILS;
}
export interface CloseUserDetails {
    type: typeof CLOSE_USER_DETAILS;
}

export interface ToggleUserDetails {
    type: typeof TOGGLE_USER_DETAILS;
}

export interface ToggleChat {
    type: typeof TOGGLE_CHAT;
}


export interface ToggleInputDisabled {
    type: typeof TOGGLE_INPUT_DISABLED;
}

export interface InputDisabled {
    type: typeof DISABLE_INPUT;
}

export interface InputEnabled {
    type: typeof ENABLE_INPUT;
}

export interface AddUserMessage {
    type: typeof ADD_NEW_USER_MESSAGE;
    text: string;
    id?: string;
}

export interface AddResponseMessage {
    type: typeof ADD_NEW_RESPONSE_MESSAGE;
    text: string;
    id?: string;
}

export interface ToggleMsgLoader {
    type: typeof TOGGLE_MESSAGE_LOADER;
}

export interface AddLinkSnippet {
    type: typeof ADD_NEW_LINK_SNIPPET;
    link: LinkParams;
    id?: string;
}

export interface RenderCustomComponent {
    type: typeof ADD_COMPONENT_MESSAGE;
    component: ElementType;
    props: any;
    showAvatar: boolean;
    id?: string;
}

export interface DropMessages {
    type: typeof DROP_MESSAGES;
}

export interface HideAvatar {
    type: typeof HIDE_AVATAR;
    index: number;
}

export interface DeleteMessages {
    type: typeof DELETE_MESSAGES;
    count: number;
    id?: string;
}

export interface SetQuickButtons {
    type: typeof SET_QUICK_BUTTONS;
    buttons: Array<{ label: string; value: string | number }>;
}

export interface SetBadgeCount {
    type: typeof SET_BADGE_COUNT;
    count: number;
}

export interface MarkAllMessagesRead {
    type: typeof MARK_ALL_READ;
}

export type BehaviorActions = OpenUserDetails | CloseUserDetails | ToggleUserDetails | ToggleChat | ToggleInputDisabled | ToggleMsgLoader | ToggleInputDisabled | InputEnabled | InputDisabled;

export type MessagesActions = AddUserMessage | AddResponseMessage | AddLinkSnippet | RenderCustomComponent | DropMessages | HideAvatar | DeleteMessages | MarkAllMessagesRead | SetBadgeCount;

export type QuickButtonsActions = SetQuickButtons;

export interface openFullscreenPreview {
    type: typeof OPEN_FULLSCREEN_PREVIEW;
    payload: FullscreenPreviewState;
}

export interface closeFullscreenPreview {
    type: typeof CLOSE_FULLSCREEN_PREVIEW;
}

export type FullscreenPreviewActions = openFullscreenPreview | closeFullscreenPreview;

export interface setContextProperties {
    type: typeof SET_CONTEXT_PROPERTIES;
    contextProperties: ContextProperties;
}

export interface getContextProperties {
    type: typeof GET_CONTEXT_PROPERTIES;
}

export interface setNameContext {
    type: typeof SET_NAME_CONTEXT;
    name: string;
}

export interface setPhoneContext {
    type: typeof SET_PHONE_CONTEXT;
    phoneNumber: string;
}

export interface setEmailAddressContext {
    type: typeof SET_EMAILADDRESS_CONTEXT;
    emailAddress: string;
}

export interface setRegionContext {
    type: typeof SET_REGION_CONTEXT;
    region: string;
}

export interface addKeyValueContext {
    type: typeof SET_KEYVALUE_CONTEXT;
    keyValue: KeyValue;
}

export interface setDynamiceResponses {
    type: typeof SET_DYNAMICRESPONSES_CONTEXT;
    dynamicResponseObject: DynamicResponses
}

export interface addNumIndividuals {
    type: typeof SET_NUM_INDIVIDUALS_CONTEXT;
    numIndividuals: number;
}

export type ContextPropertyActions = setContextProperties
| getContextProperties
| setNameContext
| setPhoneContext
| setEmailAddressContext
| setRegionContext
| addKeyValueContext
| setDynamiceResponses;

export type AllActions = ContextPropertyActions | FullscreenPreviewActions | BehaviorActions | MessagesActions | QuickButtonsActions


