import { ContextProperties, DynamicResponses, KeyValues, IMessage, CustomCompMessage, WidgetPreferences } from "@Palavyr-Types";
import { useEffect, useState } from "react";

export interface BehaviorState {
    disabledInput: boolean;
    loading: boolean;
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
    widgetPreferences: WidgetPreferences | null;
    pdfLink: string | null;
}
export interface MessagesState {
    messages: (IMessage | CustomCompMessage)[];
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
    pdfLink: null,
};

const defaultMessages: MessagesState = {
    messages: [],
    badgeCount: 0,
};

const defaultBehavior: BehaviorState = {
    disabledInput: true,
    loading: false,
    userDetailsVisible: false,
};

const defaultAppContext: AppContext = {
    ...defaultBehavior,
    ...defaultContextProperties,
    ...defaultMessages,
};

export const useAppContext = (): IAppContext => {
    const [AppContext, setAppContext] = useState<AppContext>(defaultAppContext);

    useEffect(() => {
        setAppContext(defaultAppContext);
    }, []);

    const addNewUserMessage = (message: IMessage) => {
        AppContext.messages.push(message);
        setAppContext({
            ...AppContext,
            messages: AppContext.messages,
        });
    };

    const addNewBotMessage = (message: CustomCompMessage) => {
        // MAKE SURE TO ATTACH SELECTION CUSTOM ID WHEN HITTING THE SELECTION COMPONENT SO WE CAN TRUNCATE BY IT
        AppContext.messages.push(message);
        setAppContext({
            ...AppContext,
            messages: AppContext.messages,
            badgeCount: AppContext.badgeCount + 1,
        });
    };

    const resetToSelector = () => {
        const messages = AppContext.messages;
        const indexOfSelector = messages.findIndex(m => m.nodeType === "Selection");
        const truncated = messages.slice(0, indexOfSelector);

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

    const setWidgetPreferences = (widgetPreferences: any) => {
        setAppContext({
            ...AppContext,
            widgetPreferences,
        });
    };

    const setPdfLink = (pdfLink: string) => {
        setAppContext({ ...AppContext, pdfLink });
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
        setPdfLink,
        setDynamicResponses,
        setKeyValues,

        addNewUserMessage,
        addNewBotMessage,
        resetToSelector,
        dropMessages,
        setBadgeCount,
        markAllAsRead,

        messages: AppContext.messages,
        loading: AppContext.loading,
        phoneNumber: AppContext.phoneNumber,
        emailAddress: AppContext.emailAddress,
        name: AppContext.name,
        region: AppContext.region,
        numIndividuals: AppContext.numIndividuals,
        widgetPreferences: AppContext.widgetPreferences,
        pdfLink: AppContext.pdfLink,
        dynamicResponses: AppContext.dynamicResponses,
        keyValues: AppContext.keyValues,
        disabledInput: AppContext.disabledInput,
        userDetailsVisible: AppContext.userDetailsVisible,
        badgeCount: AppContext.badgeCount,

        AppContext,
    };
};

export interface IAppContext {
    toggleInputDisable: () => void;
    toggleMessageLoader: () => void;
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
    setPdfLink: (pdfLink: string) => void;
    setDynamicResponses: (dynamicResponses: DynamicResponses) => void;
    setKeyValues: (keyValues: KeyValues) => void;

    addNewUserMessage: (message: IMessage) => void;
    addNewBotMessage: (message: CustomCompMessage) => void;
    resetToSelector: () => void;
    dropMessages: () => void;
    setBadgeCount: (badgeCount: number) => void;
    markAllAsRead: () => void;

    messages: (IMessage | CustomCompMessage)[];
    loading: boolean;
    phoneNumber: string;
    emailAddress: string;
    name: string;
    region: string;
    numIndividuals: number | null;
    widgetPreferences: WidgetPreferences | null;
    pdfLink: string | null;
    dynamicResponses: DynamicResponses;
    keyValues: KeyValues;
    disabledInput: boolean;
    userDetailsVisible: boolean;
    badgeCount: number;

    AppContext: AppContext;
}
