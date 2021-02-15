import React, { useEffect, useRef } from "react";
import { useSelector, useDispatch } from "react-redux";
import cn from "classnames";

import { GlobalState } from "../../store/types";
import { AnyFunction } from "../../utils/types";
import { _openFullscreenPreview } from "../../store/actions";

import Conversation from "./components/Conversation";
import Launcher from "./components/Launcher";
import FullScreenPreview from "./components/FullScreenPreview";

import "./style.scss";
import { WidgetPreferences } from "src/types";

type Props = {
    title: string;
    titleAvatar?: string;
    subtitle: string;
    onSendMessage: AnyFunction;
    onToggleConversation: AnyFunction;
    senderPlaceHolder: string;
    onQuickButtonClicked: AnyFunction;
    profileAvatar?: string;
    showCloseButton: boolean;
    fullScreenMode: boolean;
    autofocus: boolean;
    customLauncher?: AnyFunction;
    onTextInputChange?: (event: any) => void;
    chatId: string;
    launcherOpenLabel: string;
    launcherCloseLabel: string;
    sendButtonAlt: string;
    showTimeStamp: boolean;
    imagePreview?: boolean;
    zoomStep?: number;
    customPreferences: WidgetPreferences;
};

function WidgetLayout({
    title,
    titleAvatar,
    subtitle,
    onSendMessage,
    onToggleConversation,
    senderPlaceHolder,
    onQuickButtonClicked,
    profileAvatar,
    showCloseButton,
    fullScreenMode,
    autofocus,
    customLauncher,
    onTextInputChange,
    chatId,
    launcherOpenLabel,
    launcherCloseLabel,
    sendButtonAlt,
    showTimeStamp,
    imagePreview,
    zoomStep,
    customPreferences
}: Props) {
    const dispatch = useDispatch();
    const { dissableInput, showChat, visible } = useSelector((state: GlobalState) => ({
        showChat: state.behavior.showChat,
        dissableInput: state.behavior.disabledInput,
        visible: state.preview.visible,
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
        if (imagePreview && showChat) {
            target?.addEventListener("click", eventHandle, false);
        }

        return () => {
            target?.removeEventListener("click", eventHandle);
        };
    }, [imagePreview, showChat]);

    useEffect(() => {
        document.body.setAttribute("style", `overflow: ${visible || fullScreenMode ? "hidden" : "auto"}`);
    }, [fullScreenMode, visible]);

    return (
        <div
            className={cn("rcw-widget-container", {
                "rcw-full-screen": fullScreenMode,
                "rcw-previewer": imagePreview,
            })}
        >
            {showChat && (
                <Conversation
                    title={title}
                    subtitle={subtitle}
                    sendMessage={onSendMessage}
                    senderPlaceHolder={senderPlaceHolder}
                    profileAvatar={profileAvatar}
                    toggleChat={onToggleConversation}
                    showCloseButton={showCloseButton}
                    disabledInput={dissableInput}
                    autofocus={autofocus}
                    titleAvatar={titleAvatar}
                    className={showChat ? "active" : "hidden"}
                    onQuickButtonClicked={onQuickButtonClicked}
                    onTextInputChange={onTextInputChange}
                    sendButtonAlt={sendButtonAlt}
                    showTimeStamp={showTimeStamp}
                    customPreferences={customPreferences}
                />
            )}
            {customLauncher ? customLauncher(onToggleConversation) : !fullScreenMode && <Launcher toggle={onToggleConversation} chatId={chatId} openLabel={launcherOpenLabel} closeLabel={launcherCloseLabel} />}
            {imagePreview && <FullScreenPreview fullScreenMode={fullScreenMode} zoomStep={zoomStep} customPreferences={customPreferences} />}
        </div>
    );
}

export default WidgetLayout;
