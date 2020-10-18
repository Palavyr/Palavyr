import * as React from 'react';
import { SelectedOption, WidgetPreferences } from '../types';
import { addResponseMessage, toggleMsgLoader, setQuickButtons, Widget, isWidgetOpened, toggleWidget } from 'react-chat-widget';
import { useParams } from 'react-router-dom';
import CreateClient from '../client/Client';
import { renderNextComponent, ConvoContextProperties } from '../componentRegistry';
import { getRootNode } from '../componentRegistry/utils';
import { useState, useCallback, useEffect } from 'react';
import fetchIpData from '../region/FetchIP';


interface ICustomWidget {
    option: SelectedOption;
    preferences: WidgetPreferences;
}

export const CustomWidget = ({ option, preferences }: ICustomWidget) => {

    const { secretKey } = useParams< {secretKey: string } >();
    const client = CreateClient(secretKey);
    const [prefs, setPrefs] = useState<WidgetPreferences>();
    const [, setUserInput] = useState<string>(); // TODO: send through convo

    const initializeConvo = useCallback(async () => {

        var newConversation = await client.Widget.Access.createConvo(option.areaId);
        var nodes = newConversation.data.conversationNodes;
        var convoId = newConversation.data.conversationId;
        var region = (await fetchIpData).country;

        setPrefs(preferences);
        var rootNode = getRootNode(nodes);

        const convoContext: any = {};
        convoContext[ConvoContextProperties.DynamicResponse] = [];
        convoContext[ConvoContextProperties.KeyValues] = [];
        convoContext[ConvoContextProperties.EmailAddress] = "";
        convoContext[ConvoContextProperties.PhoneNumber] = "";
        convoContext[ConvoContextProperties.Name] = "";
        convoContext[ConvoContextProperties.Region] = region;

        renderNextComponent(rootNode, nodes, client, convoId, convoContext);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [preferences])

    useEffect(() => {
        addResponseMessage(`Welcome to this awesome chat! You chose: ${option.areaDisplay}`);
        initializeConvo();

        if (!isWidgetOpened()) toggleWidget();
        return () => {
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [initializeConvo, initializeConvo])

    const handleNewUserMessage = (newMessage: string) => {
        setUserInput(newMessage)

        toggleMsgLoader();
        setTimeout(() => {
            toggleMsgLoader();
        }, 2000);
    }

    const handleQuickButtonClicked = (e: any) => {
        addResponseMessage('Selected ' + e);
        setQuickButtons([]);
    }

    const handleSubmit = (msgText: string) => {
        return false;
    };

    return (
        <Widget
            title={prefs?.title}
            subtitle={prefs?.subtitle}
            senderPlaceHolder={prefs?.placeholder}
            handleNewUserMessage={handleNewUserMessage}
            handleQuickButtonClicked={handleQuickButtonClicked}
            imagePreview
            handleSubmit={handleSubmit}
            fullScreenMode={true}
            showCloseButton={false}
        />
    )
}
