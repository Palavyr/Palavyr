import * as React from "react";

import { getRootNode } from "../componentRegistry/utils";
import { useEffect, useRef } from "react";
import { useLocation } from "react-router-dom";
import { renderNextComponent } from "componentRegistry/renderNextComponent";
import { GlobalState, SelectedOption, WidgetPreferences } from "@Palavyr-Types";
import { useDispatch } from "react-redux";
import { Conversation } from "./components/Conversation/Conversation";

import { useSelector } from "react-redux";
import cn from "classnames";

import "./style.scss";
import { isWidgetOpened, toggleWidget } from "@store-dispatcher";
import { WidgetClient } from "client/Client";
import { _openFullscreenPreview } from "@store-actions";

export interface WidgetProps {
    option: SelectedOption;
    preferences: WidgetPreferences;
}

export const Widget = ({ option, preferences }: WidgetProps) => {
    const location = useLocation();
    const secretKey = new URLSearchParams(location.search).get("key");
    const client = new WidgetClient(secretKey);
    const dispatch = useDispatch();

    const initializeConvo = async () => {
        const { data: newConversation } = await client.Widget.Get.NewConversation(option.areaId);
        const nodes = newConversation.conversationNodes;
        const convoId = newConversation.conversationId;

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

    return preferences ? (
        <div
            className={cn("rcw-widget-container", {
                "rcw-full-screen": fullScreenMode,
            })}
        >
            {showChat && (
                <Conversation
                    title={preferences.title}
                    subtitle={preferences.subtitle}
                    senderPlaceHolder={preferences.placeholder}
                    className={showChat ? "active" : "hidden"}
                    showTimeStamp={true}
                    customPreferences={preferences}
                    // titleAvatar={titleAvatar}
                    // profileAvatar={profileAvatar}
                />
            )}
        </div>
    ) : null;
};
