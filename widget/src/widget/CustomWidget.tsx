import * as React from 'react';
import { SelectedOption, WidgetPreferences } from '../types';
import { addResponseMessage, toggleMsgLoader, setQuickButtons } from 'src/widgetCore/store/dispatcher';
import {Widget, isWidgetOpened, toggleWidget} from "src/widget";

import CreateClient from '../client/Client';
import { renderNextComponent, ConvoContextProperties } from '../componentRegistry';
import { getRootNode } from '../componentRegistry/utils';
import { useState, useCallback, useEffect } from 'react';
import fetchIpData from '../region/FetchIP';
import { useLocation } from 'react-router-dom';


interface ICustomWidget {
    option: SelectedOption;
    preferences: WidgetPreferences;
}

export const CustomWidget = ({ option, preferences }: ICustomWidget) => {

    var secretKey = (new URLSearchParams(useLocation().search)).get("key")
    const client = CreateClient(secretKey);
    const [prefs, setPrefs] = useState<WidgetPreferences>();
    const [, setUserInput] = useState<string>(); // TODO: send through convo

    const initializeConvo = useCallback(async () => {

        var {data: newConversation} = await client.Widget.Access.createConvo(option.areaId);
        var nodes = newConversation.conversationNodes;
        var convoId = newConversation.conversationId;
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
