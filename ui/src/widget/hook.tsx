import { ContextProperties, DynamicResponses, KeyValues, UserMessageData, BotMessageData, WidgetPreferencesResource, FileAssetResource, KeyValue } from "@Palavyr-Types";
import { useState } from "react";

export interface BehaviorState {
    disabledInput: boolean;
    loading: boolean;
    userDetailsVisible: boolean;
    resetEnabled: boolean;
    detailsIconEnabled: boolean;
    resetRequested: boolean;
    readingSpeed: number;
    chatStarted: boolean;
}

export interface ContextState {
    name: string;
    emailAddress: string;
    phoneNumber: string;
    region: string;
    keyValues: KeyValues;
    dynamicResponses: DynamicResponses;
    numIndividuals: number | null;
    widgetPreferences: WidgetPreferencesResource | null;
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
    detailsIconEnabled: false,
    resetRequested: false,
    readingSpeed: 2,
    chatStarted: false,
};

const defaultAppContext: AppContext = {
    ...defaultBehavior,
    ...defaultContextProperties,
    ...defaultMessages,
};

export const useAppContext = (): IAppContext => {
    const [AppContext, setAppContext] = useState<AppContext>(defaultAppContext);

    const addNewUserMessage = (message: UserMessageData) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            messages: [...appContext.messages, message],
        }));
    };

    const addNewBotMessage = (message: BotMessageData) => {
        // MAKE SURE TO ATTACH SELECTION CUSTOM ID WHEN HITTING THE SELECTION COMPONENT SO WE CAN TRUNCATE BY IT
        setAppContext((appContext: AppContext) => {
            return {
                ...appContext,
                messages: [...appContext.messages, message],
                badgeCount: appContext.badgeCount + 1,
            };
        });
    };

    const setChatStarted = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            chatStarted: true,
        }));
    };

    const enableReset = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            resetEnabled: true,
        }));
    };

    const disableReset = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            resetEnabled: false,
        }));
    };

    const resetToSelector = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            messages: [],
            loading: false,
            keyValues: [],
            dynamicResponses: [],
            numIndividuals: null,
            responseFileAsset: { fileId: "", fileName: "", link: "" },
            badgeCount: 0,
            chatStarted: false,
        }));
    };

    const dropMessages = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            messages: [],
        }));
    };

    const setBadgeCount = (badgeCount: number) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            badgeCount,
        }));
    };

    const markAllAsRead = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            messages: appContext.messages.map(m => ({ ...m, read: true })),
            badgeCount: 0,
        }));
    };

    const toggleInputDisable = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            disabledInput: !appContext.disabledInput,
        }));
    };

    const toggleMessageLoader = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            loading: !appContext.loading,
        }));
    };
    const enableMessageLoader = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            loading: true,
        }));
    };

    const disableMessageLoader = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            loading: false,
        }));
    };

    const toggleUserDetails = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            userDetailsVisible: !appContext.userDetailsVisible,
        }));
    };

    const enableDetailsIcon = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            detailsIconEnabled: true,
        }));
    };

    const openUserDetails = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            userDetailsVisible: true,
        }));
    };

    const closeUserDetails = () => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            userDetailsVisible: false,
        }));
    };

    const setContextProperties = (properties: ContextProperties) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            ...properties,
        }));
    };

    const setNumIndividuals = (numIndividuals: number) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            numIndividuals,
        }));
    };

    const setName = (name: string) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            name,
        }));
    };

    const setPhoneNumber = (phoneNumber: string) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            phoneNumber,
        }));
    };

    const setEmailAddress = (emailAddress: string) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            emailAddress,
        }));
    };

    const setRegion = (region: string) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            region,
        }));
    };

    const setWidgetPreferences = (widgetPreferences: WidgetPreferencesResource) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            widgetPreferences,
        }));
    };

    const setResponseFileAsset = (responseFileAsset: FileAssetResource) => {
        setAppContext((appContext: AppContext) => ({
            ...appContext,
            responseFileAsset: responseFileAsset,
        }));
    };

    const setDynamicResponses = (dynamicResponses: DynamicResponses) => {
        setAppContext((appContext: AppContext) => {
            return {
                ...appContext,
                dynamicResponses: [...appContext.dynamicResponses, ...dynamicResponses],
            };
        });
    };

    const addKeyValue = (keyValue: KeyValue) => {
        setAppContext((appContext: AppContext) => ({ ...appContext, keyValues: [...appContext.keyValues, keyValue] }));
    };

    const requestReset = () => {
        setAppContext((appContext: AppContext) => ({ ...appContext, resetRequested: true }));
    };

    const setReadingSpeed = (speed: number) => {
        setAppContext((appContext: AppContext) => ({ ...appContext, readingSpeed: speed }));
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
        addKeyValue,

        addNewUserMessage,
        addNewBotMessage,
        resetToSelector,
        dropMessages,
        setBadgeCount,
        markAllAsRead,
        enableReset,
        disableReset,
        setChatStarted,
        requestReset,
        enableDetailsIcon,
        setReadingSpeed,

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
        detailsIconEnabled: AppContext.detailsIconEnabled,
        resetRequested: AppContext.resetRequested,
        readingSpeed: AppContext.readingSpeed,
        chatStarted: AppContext.chatStarted,
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
    addKeyValue: (keyValue: KeyValue) => void;

    addNewUserMessage: (message: UserMessageData) => void;
    addNewBotMessage: (message: BotMessageData) => void;
    resetToSelector: () => void;
    requestReset: () => void;
    dropMessages: () => void;
    setBadgeCount: (badgeCount: number) => void;
    markAllAsRead: () => void;
    enableReset(): void;
    disableReset(): void;
    setChatStarted(): void;
    enableDetailsIcon(): void;

    setReadingSpeed(speed: number): void;

    messages: (UserMessageData | BotMessageData)[];
    loading: boolean;
    phoneNumber: string;
    emailAddress: string;
    name: string;
    region: string;
    numIndividuals: number | null;
    widgetPreferences: WidgetPreferencesResource | null;
    responseFileAsset: FileAssetResource | null;
    dynamicResponses: DynamicResponses;
    keyValues: KeyValues;
    disabledInput: boolean;
    userDetailsVisible: boolean;
    badgeCount: number;
    resetEnabled: boolean;
    detailsIconEnabled: boolean;
    resetRequested: boolean;
    readingSpeed: number;
    chatStarted: boolean;
    AppContext: AppContext;
}
