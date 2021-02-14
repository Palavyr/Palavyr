import { ElementType } from "react";

type BaseMessage = {
    type: string;
    component: ElementType;
    sender: string;
    showAvatar: boolean;
    timestamp: Date;
    unread: boolean;
    customId?: string;
    props?: any;
};

export type DynamicResponse = {
    [key: string]: string;
};
export type DynamicResponses = Array<DynamicResponse>;

export type KeyValue = {
    [key: string]: string;
};

export type KeyValues = Array<KeyValue>;

export type ContextProperties = {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
};

export interface Message extends BaseMessage {
    text: string;
}

export type QuickButton = {
    label: string;
    value: string | number;
    component: ElementType;
};

export interface Link extends BaseMessage {
    title: string;
    link: string;
    target: string;
}

export interface LinkParams {
    link: string;
    title: string;
    target?: string;
}

export interface CustomCompMessage extends BaseMessage {
    props: any;
}

export interface BehaviorState {
    showChat: boolean;
    disabledInput: boolean;
    messageLoader: boolean;
}

export interface ContextState {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
}

export interface MessagesState {
    messages: (Message | Link | CustomCompMessage)[];
    badgeCount: number;
}

export interface QuickButtonsState {
    quickButtons: QuickButton[];
}

export interface ImageState {
    src: string;
    alt?: string;
    width: number;
    height: number;
}

export interface FullscreenPreviewState extends ImageState {
    visible?: boolean;
}

export interface GlobalState {
    messages: MessagesState;
    behavior: BehaviorState;
    quickButtons: QuickButtonsState;
    preview: FullscreenPreviewState;
    context: ContextProperties;
}
