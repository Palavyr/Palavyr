import React from "react";
import { WidgetPreferences } from "src/types";
import Widget from "./components/Widget";
import { AnyFunction } from "./utils/types";

type Props = {
    handleNewUserMessage: AnyFunction;
    handleQuickButtonClicked?: AnyFunction;
    title?: string;
    titleAvatar?: string;
    subtitle?: string;
    senderPlaceHolder?: string;
    showCloseButton?: boolean;
    fullScreenMode?: boolean;
    autofocus?: boolean;
    profileAvatar?: string;
    launcher?: AnyFunction;
    handleTextInputChange?: (event: any) => void;
    chatId?: string;
    launcherOpenLabel?: string;
    launcherCloseLabel?: string;
    sendButtonAlt?: string;
    showTimeStamp?: boolean;
    imagePreview?: boolean;
    zoomStep?: number;
    handleSubmit?: AnyFunction;
    customPreferences: WidgetPreferences;
} & typeof defaultProps;

function ConnectedWidget({
    title,
    titleAvatar,
    subtitle,
    senderPlaceHolder,
    showCloseButton,
    fullScreenMode,
    autofocus,
    profileAvatar,
    launcher,
    handleNewUserMessage,
    handleQuickButtonClicked,
    handleTextInputChange,
    chatId,
    launcherOpenLabel,
    launcherCloseLabel,
    sendButtonAlt,
    showTimeStamp,
    imagePreview,
    zoomStep,
    handleSubmit,
    customPreferences,
}: Props) {
    return (
        <Widget
            title={title}
            titleAvatar={titleAvatar}
            subtitle={subtitle}
            handleNewUserMessage={handleNewUserMessage}
            handleQuickButtonClicked={handleQuickButtonClicked}
            senderPlaceHolder={senderPlaceHolder}
            profileAvatar={profileAvatar}
            showCloseButton={showCloseButton}
            fullScreenMode={fullScreenMode}
            autofocus={autofocus}
            customLauncher={launcher}
            handleTextInputChange={handleTextInputChange}
            chatId={chatId}
            launcherOpenLabel={launcherOpenLabel}
            launcherCloseLabel={launcherCloseLabel}
            sendButtonAlt={sendButtonAlt}
            showTimeStamp={showTimeStamp}
            imagePreview={imagePreview}
            zoomStep={zoomStep}
            handleSubmit={handleSubmit}
            customPreferences={customPreferences}
        />
    );
}

const defaultProps = {
    title: "Welcome",
    subtitle: "This is your chat subtitle",
    senderPlaceHolder: "Type a message...",
    showCloseButton: true,
    fullScreenMode: false,
    autofocus: true,
    chatId: "rcw-chat-container",
    launcherOpenLabel: "Open chat",
    launcherCloseLabel: "Close chat",
    sendButtonAlt: "Send",
    showTimeStamp: true,
    imagePreview: false,
    zoomStep: 80,
};
ConnectedWidget.defaultProps = defaultProps;

export default ConnectedWidget;
