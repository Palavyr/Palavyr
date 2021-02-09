import React from 'react';
import cn from 'classnames';

import Header from './components/Header';
import Messages from './components/Messages';
import Sender from './components/Sender';
import QuickButtons from './components/QuickButtons';

import { AnyFunction } from '../../../../utils/types';

import './style.scss';
import { WidgetPreferences } from 'src/types';

type Props = {
  title: string;
  subtitle: string;
  senderPlaceHolder: string;
  showCloseButton: boolean;
  disabledInput: boolean;
  autofocus: boolean;
  className: string;
  sendMessage: AnyFunction;
  toggleChat: AnyFunction;
  profileAvatar?: string;
  titleAvatar?: string;
  onQuickButtonClicked?: AnyFunction;
  onTextInputChange?: (event: any) => void;
  sendButtonAlt: string;
  showTimeStamp: boolean;
  customPreferences: WidgetPreferences;
  openUserDetails: any;
};

function Conversation({
  title,
  subtitle,
  senderPlaceHolder,
  showCloseButton,
  disabledInput,
  autofocus,
  className,
  sendMessage,
  toggleChat,
  profileAvatar,
  titleAvatar,
  onQuickButtonClicked,
  onTextInputChange,
  sendButtonAlt,
  showTimeStamp,
  customPreferences,
  openUserDetails
}: Props) {
  return (
    <div className={cn('rcw-conversation-container', className)} aria-live="polite">
      <Header
        title={title}
        subtitle={subtitle}
        toggleChat={toggleChat}
        showCloseButton={showCloseButton}
        titleAvatar={titleAvatar}
        customPreferences={customPreferences}
        openUserDetails={openUserDetails}
      />
      <Messages profileAvatar={profileAvatar} showTimeStamp={showTimeStamp} customPreferences={customPreferences}/>
      {/* <QuickButtons onQuickButtonClicked={onQuickButtonClicked} customPreferences={customPreferences} /> */}
      <Sender
        sendMessage={sendMessage}
        placeholder={senderPlaceHolder}
        disabledInput={disabledInput}
        autofocus={autofocus}
        onTextInputChange={onTextInputChange}
        buttonAlt={sendButtonAlt}
      />
    </div>
  );
}

export default Conversation;
