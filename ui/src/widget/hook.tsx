import { ContextProperties, MessagesState, BehaviorState, DynamicResponses, KeyValues, IMessage, CustomCompMessage } from "@Palavyr-Types";
import { useEffect, useState } from "react";
import { AppContext } from "widget";

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

export const useAppContext = () => {
    const [AppContext, setAppContext] = useState<AppContext>(defaultAppContext);

    useEffect(() => {
        setAppContext(defaultAppContext);
    }, [AppContext]);

    const addNewUserMessage = (message: IMessage) => {
        setAppContext({
            ...AppContext,
            messages: [...AppContext.messages, message],
        });
    };

    const addNewBotMessage = (message: CustomCompMessage) => {
        // MAKE SURE TO ATTACH SELECTION CUSTOM ID WHEN HITTING THE SELECTION COMPONENT SO WE CAN TRUNCATE BY IT
        setAppContext({
            ...AppContext,
            messages: [...AppContext.messages, message],
            badgeCount: AppContext.badgeCount + 1,
        });
    };

    const resetToSelector = () => {
        const messages = AppContext.messages;
        const indexOfSelector = messages.findIndex(m => m.customId === "Selection");

        setAppContext({
            ...AppContext,
            messages: [],
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
    };
};
