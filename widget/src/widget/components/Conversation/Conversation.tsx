import React from "react";
import cn from "classnames";

import { Messages } from "../Messages/Messages";

import "./style.scss";
import { WidgetPreferences } from "@Palavyr-Types";
import { ConvoHeader } from "../ConvoHeader/ConvoHeader";

type Props = {
    title: string;
    subtitle: string;
    className: string;
    senderPlaceHolder: string;
    profileAvatar?: string;
    titleAvatar?: string;
    showTimeStamp: boolean;
    customPreferences: WidgetPreferences;
};

export const Conversation = ({ title, subtitle, className, profileAvatar, titleAvatar, showTimeStamp, customPreferences }: Props) => {
    return (
        <div className={cn("rcw-conversation-container", className)} aria-live="polite">
            <ConvoHeader title={title} subtitle={subtitle} titleAvatar={titleAvatar} headerColor={customPreferences.headerColor} headerFontColor={customPreferences.headerFontColor} />
            <Messages profileAvatar={profileAvatar} showTimeStamp={showTimeStamp} customPreferences={customPreferences} />
        </div>
    );
};
