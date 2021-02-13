import * as React from "react";
import { ContextProperties, SelectedOption, WidgetPreferences } from "../types";
import { addResponseMessage, toggleMsgLoader, setQuickButtons } from "src/widgetCore/store/dispatcher";
import { Widget, isWidgetOpened, toggleWidget } from "src/widget";

import CreateClient from "../client/Client";
import { renderNextComponent } from "../componentRegistry";
import { getRootNode } from "../componentRegistry/utils";
import { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { Dispatch } from "react";
import { SetStateAction } from "react";

interface ICustomWidget {
    setUserDetailsDialogState: any;
    contextProperties: ContextProperties;
    setContextProperties: Dispatch<SetStateAction<ContextProperties>>;
    option: SelectedOption;
    preferences: WidgetPreferences;
    initialDialogState: boolean;
};

export const CustomWidget = ({ initialDialogState, setUserDetailsDialogState, contextProperties, setContextProperties, option, preferences }: ICustomWidget) => {
    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const client = CreateClient(secretKey);
    const [prefs, setPrefs] = useState<WidgetPreferences>();
    const [, setUserInput] = useState<string>(); // TODO: send through convo

    const initializeConvo = async () => {
        const { data: newConversation } = await client.Widget.Access.createConvo(option.areaId);
        const nodes = newConversation.conversationNodes;
        const convoId = newConversation.conversationId;

        setPrefs(preferences);
        const rootNode = getRootNode(nodes);

        renderNextComponent(rootNode, nodes, client, convoId, contextProperties, setContextProperties);

    };

    useEffect(() => {
        (async () => {
            await initializeConvo();
        })();

        if (!isWidgetOpened()) toggleWidget();
        return () => {};
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const handleNewUserMessage = (newMessage: string) => {
        setUserInput(newMessage);

        toggleMsgLoader();
        setTimeout(() => {
            toggleMsgLoader();
        }, 2000);
    };

    const handleQuickButtonClicked = (e: any) => {
        addResponseMessage("Selected " + e);
        setQuickButtons([]);
    };

    const handleSubmit = (msgText: string) => {
        return false;
    };
    // const tempAvatar = "C:PalavyrDataUserDataa6d4ad3b-efd8AccountLogoa0e31ee9-a120-4aca-8a02-9097f794c8f1.svg";
    return (
        <Widget
            openUserDetails={setUserDetailsDialogState}
            title={prefs?.title}
            subtitle={prefs?.subtitle}
            senderPlaceHolder={prefs?.placeholder}
            handleNewUserMessage={handleNewUserMessage}
            handleQuickButtonClicked={handleQuickButtonClicked}
            imagePreview
            handleSubmit={handleSubmit}
            fullScreenMode={true}
            showCloseButton={false}
            customPreferences={preferences}
            // profileAvatar={tempAvatar}
            showTimeStamp={true}
            // titleAvatar={tempAvatar}
        />
    );
};
