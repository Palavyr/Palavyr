import * as React from "react";

import { useEffect, useRef } from "react";
import { useLocation } from "react-router-dom";
import { renderNextComponent } from "widget/BotResponse/utils/renderNextComponent";
import { GlobalState, SelectedOption } from "@Palavyr-Types";
import { useDispatch } from "react-redux";

import { useSelector } from "react-redux";
import cn from "classnames";

import { getContextProperties, getEmailAddressContext, getNameContext, getPhoneContext, getRegionContext, isWidgetOpened, toggleWidget } from "@store-dispatcher";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { _openFullscreenPreview } from "@store-actions";
import { ConvoContextProperties } from "widget/componentRegistry/registry";
import "./style.scss";
import { getRootNode } from "widget/BotResponse/utils/utils";
import { Conversation } from "widget/components/Conversation/Conversation";

export interface WidgetProps {
    option: SelectedOption;
}

export const Widget = ({ option }: WidgetProps) => {
    const location = useLocation();
    const secretKey = new URLSearchParams(location.search).get("key");
    const client = new PalavyrWidgetRepository(secretKey);
    const dispatch = useDispatch();

    // this thing
    const initializeConvo = async () => {
        const newConversation = await client.Widget.Get.NewConversation(option.areaId, {
            Name: getNameContext(),
            Email: getEmailAddressContext(),
            Locale: getRegionContext(),
            PhoneNumber: getPhoneContext(),
        });
        const nodes = newConversation.conversationNodes;
        const convoId = newConversation.conversationId;

        const contextProperties = getContextProperties();

        const name = contextProperties[ConvoContextProperties.name];
        const phone = contextProperties[ConvoContextProperties.phoneNumber];
        const email = contextProperties[ConvoContextProperties.emailAddress];
        const locale = contextProperties[ConvoContextProperties.region];

        await client.Widget.Post.UpdateConvoRecord({
            Name: name,
            PhoneNumber: phone,
            Email: email,
            Locale: locale,
            ConversationId: convoId,
        });

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

    const fullScreenMode = true;
    const { showChat, visible } = useSelector((state: GlobalState) => ({
        showChat: state.behaviorReducer.showChat,
        visible: true,
    }));

    const messageRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        if (showChat) {
            messageRef.current = document.getElementById("messages") as HTMLDivElement;
        }
        return () => {
            messageRef.current = null;
        };
    }, [showChat]);

    const eventHandle = evt => {
        if (evt.target && evt.target.className === "rcw-message-img") {
            const { src, alt, naturalWidth, naturalHeight } = evt.target as HTMLImageElement;
            const obj = {
                src: src,
                alt: alt,
                width: naturalWidth,
                height: naturalHeight,
            };
            dispatch(_openFullscreenPreview(obj));
        }
    };

    /**
     * Previewer needs to prevent body scroll behavior when fullScreenMode is true
     */
    useEffect(() => {
        const target = messageRef?.current;
        if (showChat) {
            target?.addEventListener("click", eventHandle, false);
        }

        return () => {
            target?.removeEventListener("click", eventHandle);
        };

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [showChat]);

    useEffect(() => {
        document.body.setAttribute("style", `overflow: ${visible || fullScreenMode ? "hidden" : "auto"}`);
    }, [fullScreenMode, visible]);

    return (
        <div
            className={cn("rcw-widget-container", {
                "rcw-full-screen": fullScreenMode,
            })}
        >
            {showChat && <Conversation className={showChat ? "active" : "hidden"} showTimeStamp={true} />}
        </div>
    );
};
