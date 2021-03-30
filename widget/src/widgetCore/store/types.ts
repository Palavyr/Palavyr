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

export type DynamicResponses = DynamicResponse[];
export type DynamicResponse = {
    [UniqueTableKey: string]: Response[];
};
export type Response = {
    [nodeId: string]: string; // node.nodeId: response value;
};


export type KeyValue = {
    [key: string]: string; // MUST BE STRING for server
};

export type KeyValues = Array<KeyValue>;


export type ContextProperties = {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
    numIndividuals: number | null;
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
    userDetailsVisible: boolean;
}

export interface ContextState {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
    numIndividuals: number | null;
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
