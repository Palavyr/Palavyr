import { PalavyrWidgetRepository } from "@api-client/PalavyrWidgetRepository";
import { ElementType } from "react";
import { FileAssetResource, IntentResource, WidgetNodeResource, WidgetNodeResources, WidgetPreferencesResource } from "../api/EntityResources";

export type SecretKey = string | null;

export type WidgetIntentTable = {
    intentId: string;
    intentDisplayTitle: string;
};

export type SelectedOption = {
    intentDisplay: string;
    intentId: string;
};

export type Registry = {
    [key: string]: any;
};

export type ConversationRecordUpdate = {
    ConversationId: string;
    IntentId: string;
    Name: string;
    Email: string;
    PhoneNumber: string;
    Fallback: boolean;
    Locale: string;
    IsComplete: boolean;
};

export type WidgetPreCheckResult = {
    isReady: boolean;
    incompleteIntents: IntentResource[];
};

export interface IProgressTheChat {
    node: WidgetNodeResource;
    nodeList: WidgetNodeResources;
    client: PalavyrWidgetRepository;
    convoId: string;
    designer?: boolean;
}

export type Nullable<T> = T | null;

type BaseMessage = {
    type: string;
    component: ElementType;
    sender: string;
    showAvatar: boolean;
    timestamp: Date;
    unread: boolean;
    customId?: string;
    props?: any;
    nodeType: string;
};
export interface UserMessageData extends BaseMessage {
    text: string;
}

export interface BotMessageData extends BaseMessage {
    props: any;
}

export type SpecificResponse = {
    [nodeId: string]: string; // node.nodeId: response value;
};
export type DynamicResponse = {
    [UniqueTableKey: string]: SpecificResponse[];
};
export type DynamicResponses = DynamicResponse[];

export type KeyValue = {
    [key: string]: string; // MUST BE STRING for server
};

export type KeyValues = KeyValue[];

export type ContextProperties = {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
    numIndividuals: number | null;
    widgetPreferences: WidgetPreferencesResource | null;
    responseFileAsset: FileAssetResource;
};
