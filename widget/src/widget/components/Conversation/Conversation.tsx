import React from "react";
import cn from "classnames";

import { Messages } from "../Messages/Messages";

import "./style.scss";
import { ConvoHeader } from "../ConvoHeader/ConvoHeader";

type ConversationProps = {
    className: string;
    profileAvatar?: string;
    titleAvatar?: string;
    showTimeStamp: boolean;
};

export const Conversation = ({ className, profileAvatar, titleAvatar, showTimeStamp }: ConversationProps) => {
    return (
        <div className={cn("rcw-conversation-container", className)} aria-live="polite">
            <ConvoHeader titleAvatar={titleAvatar} />
            <Messages profileAvatar={profileAvatar} showTimeStamp={showTimeStamp} />
        </div>
    );
};
