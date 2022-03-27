import { ContextProperties, DynamicResponses, KeyValues, UserMessageData, BotMessageData, WidgetPreferences, FileAssetResource } from "@Palavyr-Types";
import { useState } from "react";

export interface BehaviorState {
    disabledInput: boolean;
    loading: boolean;
    userDetailsVisible: boolean;
    resetEnabled: boolean;
}

export interface ContextState {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
    numIndividuals: number | null;
    widgetPreferences: WidgetPreferences | null;
    responseFileAsset: FileAssetResource | null;
}
export interface MessagesState {
    messages: (UserMessageData | BotMessageData)[];
    badgeCount: number;
}

export interface AppContext extends BehaviorState, ContextState, MessagesState {}

const defaultContextProperties: ContextProperties = {
    dynamicResponses: [],
    keyValues: [],
    emailAddress: "",
    phoneNumber: "",
    name: "",
    region: "",
    numIndividuals: null,
    widgetPreferences: null,
    responseFileAsset: { fileId: "", fileName: "", link: "" },
};

const defaultMessages: MessagesState = {
    messages: [],
    badgeCount: 0,
};

const defaultBehavior: BehaviorState = {
    disabledInput: true,
    loading: false,
    userDetailsVisible: false,
    resetEnabled: false,
};

const defaultAppContext: AppContext = {
    ...defaultBehavior,
    ...defaultContextProperties,
    ...defaultMessages,
};

export const useAppContext = (): IAppContext => {
    const [AppContext, setAppContext] = useState<AppContext>(defaultAppContext);

    const addNewUserMessage = (message: UserMessageData) => {
        AppContext.messages.push(message);
        setAppContext({
            ...AppContext,
            messages: [...AppContext.messages],
        });
    };

    const addNewBotMessage = (message: BotMessageData) => {
        // MAKE SURE TO ATTACH SELECTION CUSTOM ID WHEN HITTING THE SELECTION COMPONENT SO WE CAN TRUNCATE BY IT
        AppContext.messages.push(message);
        setAppContext({
            ...AppContext,
            messages: [...AppContext.messages],
            badgeCount: AppContext.badgeCount + 1,
        });
    };

    const enableReset = () => {
        console.log("enableReset");
        setAppContext({
            ...AppContext,
            resetEnabled: true,
        });
    };

    const resetToSelector = () => {
        const messages = AppContext.messages;
        const indexOfSelector = messages.findIndex(m => m.nodeType === "Selection");
        const truncated = messages.slice(0, indexOfSelector + 1);

        setAppContext({
            ...AppContext,
            messages: truncated,
            badgeCount: 0,
        });
    };

    const dropMessages = () => {
        setAppContext({
            ...AppContext,
            messages: [],
            badgeCount: 0,
        });
    };

    const setBadgeCount = (badgeCount: number) => {
        setAppContext({
            ...AppContext,
            badgeCount,
        });
    };

    const markAllAsRead = () => {
        setAppContext({
            ...AppContext,
            messages: AppContext.messages.map(m => ({ ...m, read: true })),
            badgeCount: 0,
        });
    };

    const toggleInputDisable = () => {
        setAppContext({
            ...AppContext,
            disabledInput: !AppContext.disabledInput,
        });
    };

    const toggleMessageLoader = () => {
        setAppContext({
            ...AppContext,
            loading: !AppContext.loading,
        });
    };
    const enableMessageLoader = () => {
        setAppContext({
            ...AppContext,
            loading: true,
        });
    };

    const disableMessageLoader = () => {
        setAppContext({
            ...AppContext,
            loading: false,
        });
    };

    const toggleUserDetails = () => {
        setAppContext({
            ...AppContext,
            userDetailsVisible: !AppContext.userDetailsVisible,
        });
    };

    const openUserDetails = () => {
        setAppContext({
            ...AppContext,
            userDetailsVisible: true,
        });
    };

    const closeUserDetails = () => {
        setAppContext({
            ...AppContext,
            userDetailsVisible: false,
        });
    };

    const setContextProperties = (properties: ContextProperties) => {
        setAppContext({
            ...AppContext,
            ...properties,
        });
    };

    const setNumIndividuals = (numIndividuals: number) => {
        setAppContext({
            ...AppContext,
            numIndividuals,
        });
    };

    const setName = (name: string) => {
        setAppContext({
            ...AppContext,
            name,
        });
    };

    const setPhoneNumber = (phoneNumber: string) => {
        setAppContext({
            ...AppContext,
            phoneNumber,
        });
    };

    const setEmailAddress = (emailAddress: string) => {
        setAppContext({
            ...AppContext,
            emailAddress,
        });
    };

    const setRegion = (region: string) => {
        setAppContext({
            ...AppContext,
            region,
        });
    };

    const setWidgetPreferences = (widgetPreferences: WidgetPreferences) => {
        setAppContext({
            ...AppContext,
            widgetPreferences,
        });
    };

    const setResponseFileAsset = (responseFileAsset: FileAssetResource) => {
        AppContext.responseFileAsset = responseFileAsset;
        setAppContext({
            ...AppContext,
            responseFileAsset: AppContext.responseFileAsset,
        });
    };

    const setDynamicResponses = (dynamicResponses: DynamicResponses) => {
        setAppContext({ ...AppContext, dynamicResponses });
    };

    const setKeyValues = (keyValues: KeyValues) => {
        setAppContext({ ...AppContext, keyValues });
    };

    return {
        toggleInputDisable,
        toggleMessageLoader,
        enableMessageLoader,
        disableMessageLoader,

        toggleUserDetails,
        openUserDetails,
        closeUserDetails,

        setContextProperties,
        setNumIndividuals,
        setName,
        setPhoneNumber,
        setEmailAddress,
        setRegion,
        setWidgetPreferences,
        setResponseFileAsset,
        setDynamicResponses,
        setKeyValues,

        addNewUserMessage,
        addNewBotMessage,
        resetToSelector,
        dropMessages,
        setBadgeCount,
        markAllAsRead,
        enableReset,

        messages: AppContext.messages,
        loading: AppContext.loading,
        phoneNumber: AppContext.phoneNumber,
        emailAddress: AppContext.emailAddress,
        name: AppContext.name,
        region: AppContext.region,
        numIndividuals: AppContext.numIndividuals,
        widgetPreferences: AppContext.widgetPreferences,
        responseFileAsset: AppContext.responseFileAsset,
        dynamicResponses: AppContext.dynamicResponses,
        keyValues: AppContext.keyValues,
        disabledInput: AppContext.disabledInput,
        userDetailsVisible: AppContext.userDetailsVisible,
        badgeCount: AppContext.badgeCount,
        resetEnabled: AppContext.resetEnabled,
        AppContext,
    };
};

export interface IAppContext {
    toggleInputDisable: () => void;

    toggleMessageLoader: () => void;
    enableMessageLoader: () => void;
    disableMessageLoader: () => void;

    toggleUserDetails: () => void;
    openUserDetails: () => void;
    closeUserDetails: () => void;

    setContextProperties: (properties: ContextProperties) => void;
    setNumIndividuals: (numIndividuals: number) => void;
    setName: (name: string) => void;
    setPhoneNumber: (phoneNumber: string) => void;
    setEmailAddress: (emailAddress: string) => void;
    setRegion: (region: string) => void;
    setWidgetPreferences: (widgetPreferences: any) => void;
    setResponseFileAsset: (responseFileAssetResource: FileAssetResource) => void;
    setDynamicResponses: (dynamicResponses: DynamicResponses) => void;
    setKeyValues: (keyValues: KeyValues) => void;

    addNewUserMessage: (message: UserMessageData) => void;
    addNewBotMessage: (message: BotMessageData) => void;
    resetToSelector: () => void;
    dropMessages: () => void;
    setBadgeCount: (badgeCount: number) => void;
    markAllAsRead: () => void;
    enableReset(): void;

    messages: (UserMessageData | BotMessageData)[];
    loading: boolean;
    phoneNumber: string;
    emailAddress: string;
    name: string;
    region: string;
    numIndividuals: number | null;
    widgetPreferences: WidgetPreferences | null;
    responseFileAsset: FileAssetResource | null;
    dynamicResponses: DynamicResponses;
    keyValues: KeyValues;
    disabledInput: boolean;
    userDetailsVisible: boolean;
    badgeCount: number;
    resetEnabled: boolean;
    AppContext: AppContext;
}
