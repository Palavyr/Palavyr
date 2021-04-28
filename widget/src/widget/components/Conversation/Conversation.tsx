import React from "react";
import cn from "classnames";

import { Messages } from "../Messages/Messages";

import "./style.scss";
import { WidgetPreferences } from "@Palavyr-Types";
import { ConvoHeader } from "../ConvoHeader/ConvoHeader";

type ConversationProps = {
    className: string;
    profileAvatar?: string;
    titleAvatar?: string;
    showTimeStamp: boolean;
    customPreferences: WidgetPreferences;
};

export const Conversation = ({ className, profileAvatar, titleAvatar, showTimeStamp, customPreferences }: ConversationProps) => {
    return (
        <div className={cn("rcw-conversation-container", className)} aria-live="polite">
            <ConvoHeader chatHeader={customPreferences.chatHeader} titleAvatar={titleAvatar} preferences={customPreferences} />
            <Messages profileAvatar={profileAvatar} showTimeStamp={showTimeStamp} customPreferences={customPreferences} />
        </div>
    );
};
