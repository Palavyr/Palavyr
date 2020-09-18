import * as React from 'react';
import { SelectedOption, Preferences } from '../types';
import { addResponseMessage, toggleMsgLoader, setQuickButtons, Widget, isWidgetOpened, toggleWidget } from 'react-chat-widget';
import { useParams } from 'react-router-dom';
import CreateClient from '../client/Client';
import { renderNextComponent, ConvoContextProperties } from '../componentRegistry';
import { getRootNode } from '../componentRegistry/utils';
import { uuid } from 'uuidv4';
import { useState, useCallback, useEffect } from 'react';
import fetchIpData from '../region/FetchIP';


interface ICustomWidget {
    option: SelectedOption;
}

export const CustomWidget = ({ option }: ICustomWidget) => {

    const { secretKey } = useParams< {secretKey: string } >();
    const client = CreateClient(secretKey);
    const [prefs, setPrefs] = useState<Preferences>();
    const [, setUserInput] = useState<string>(); // TODO: send through convo

    const initializeConvo = useCallback(async () => {
        var Nodes = await client.Widget.Access.fetchNodes(option.areaId)
        var Prefs = await client.Widget.Access.fetchPreferences()
        var Region = (await fetchIpData).country;

        setPrefs(Prefs.data);

        var newConvoId = uuid();

        var rootNode = getRootNode(Nodes.data);

        const convoContext: any = {};
        convoContext[ConvoContextProperties.DynamicResponse] = [];
        convoContext[ConvoContextProperties.KeyValues] = [];
        convoContext[ConvoContextProperties.EmailAddress] = "";
        convoContext[ConvoContextProperties.PhoneNumber] = "";
        convoContext[ConvoContextProperties.Name] = "";
        convoContext[ConvoContextProperties.Region] = Region;

        renderNextComponent(rootNode, Nodes.data, client, newConvoId, convoContext);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {
        addResponseMessage(`Welcome to this awesome chat! You chose: ${option.areaDisplay}`);
        initializeConvo();

        if (!isWidgetOpened()) toggleWidget();
        return () => {
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [initializeConvo])

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
