import React from "react";
import cn from "classnames";

import { Messages } from "../Messages/Messages";

import "./style.scss";
import { ConvoHeader } from "../ConvoHeader/ConvoHeader";
import { getWidgetPreferences } from "@store-dispatcher";

type ConversationProps = {
    className: string;
    profileAvatar?: string;
    titleAvatar?: string;
    showTimeStamp: boolean;
};

export const Conversation = ({ className, profileAvatar, titleAvatar, showTimeStamp }: ConversationProps) => {
    const preferences = getWidgetPreferences();
    return preferences ? (
        <div className={cn("rcw-conversation-container", className)} aria-live="polite">
            <ConvoHeader titleAvatar={titleAvatar} preferences={preferences} />
            <Messages profileAvatar={profileAvatar} showTimeStamp={showTimeStamp} preferences={preferences} />
        </div>
    ) : null;
};
