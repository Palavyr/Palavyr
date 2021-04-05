import * as React from "react";
import { SelectedOption, WidgetPreferences } from "../types";
import { addResponseMessage, toggleMsgLoader, setQuickButtons } from "src/widgetCore/store/dispatcher";
import { Widget, isWidgetOpened, toggleWidget } from "src/widget";

import CreateClient from "../client/Client";
import { getRootNode } from "../componentRegistry/utils";
import { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { renderNextComponent } from "src/componentRegistry/renderNextComponent";

interface ICustomWidget {
    option: SelectedOption;
    preferences: WidgetPreferences;
}

export const CustomWidget = ({ option, preferences }: ICustomWidget) => {
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

        renderNextComponent(rootNode, nodes, client, convoId);
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
            customPreferences={preferences}
            // profileAvatar={tempAvatar}
            showTimeStamp={true}
            // titleAvatar={tempAvatar}
        />
    );
};
