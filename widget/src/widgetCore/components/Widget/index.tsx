import React from 'react';
import { useDispatch } from 'react-redux';
import { WidgetPreferences } from 'src/types';

import { toggleChat, addUserMessage } from '../../store/actions';
import { AnyFunction } from '../../utils/types';

import WidgetLayout from './layout';

type Props = {
  title: string;
  titleAvatar?: string;
  subtitle: string;
  senderPlaceHolder: string;
  profileAvatar?: string;
  showCloseButton: boolean;
  fullScreenMode: boolean;
  autofocus: boolean;
  customLauncher?: AnyFunction;
  handleNewUserMessage: AnyFunction;
  handleQuickButtonClicked?: AnyFunction;
  handleTextInputChange?: (event: any) => void;
  chatId: string;
  launcherOpenLabel: string;
  launcherCloseLabel: string;
  sendButtonAlt: string;
  showTimeStamp: boolean;
  imagePreview?: boolean;
  zoomStep?: number;
  handleSubmit?: AnyFunction;
  customPreferences: WidgetPreferences
}

function Widget({
  title,
  titleAvatar,
  subtitle,
  senderPlaceHolder,
  profileAvatar,
  showCloseButton,
  fullScreenMode,
  autofocus,
  customLauncher,
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
  customPreferences
}: Props) {
  const dispatch = useDispatch();

  const toggleConversation = () => {
    dispatch(toggleChat());
  }

  const handleMessageSubmit = (event) => {
    event.preventDefault();
    const userInput = event.target.message.value;

    if (!userInput.trim()) {
      return;
    }

    handleSubmit?.(userInput);
    dispatch(addUserMessage(userInput));
    handleNewUserMessage(userInput);
    event.target.message.value = '';
  }

  const onQuickButtonClicked = (event, value) => {
    event.preventDefault();
    handleQuickButtonClicked?.(value)
  }

  return (
    <WidgetLayout
      onToggleConversation={toggleConversation}
      onSendMessage={handleMessageSubmit}
      onQuickButtonClicked={onQuickButtonClicked}
      title={title}
      titleAvatar={titleAvatar}
      subtitle={subtitle}
      senderPlaceHolder={senderPlaceHolder}
      profileAvatar={profileAvatar}
      showCloseButton={showCloseButton}
      fullScreenMode={fullScreenMode}
      autofocus={autofocus}
      customLauncher={customLauncher}
      onTextInputChange={handleTextInputChange}
      chatId={chatId}
      launcherOpenLabel={launcherOpenLabel}
      launcherCloseLabel={launcherCloseLabel}
      sendButtonAlt={sendButtonAlt}
      showTimeStamp={showTimeStamp}
      imagePreview={imagePreview}
      zoomStep={zoomStep}
      customPreferences={customPreferences}
    />
  );
}

export default Widget;
