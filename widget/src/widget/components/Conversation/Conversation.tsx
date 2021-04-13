import React from "react";
import cn from "classnames";

import Messages from "../Messages/Messages";

import "./style.scss";
import { AnyFunction } from "@Palavyr-Types";
import { WidgetPreferences } from "@Palavyr-Types";
import { Header } from "../Header/Header";

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
            <Header title={title} subtitle={subtitle} titleAvatar={titleAvatar} customPreferences={customPreferences} />
            <Messages profileAvatar={profileAvatar} showTimeStamp={showTimeStamp} customPreferences={customPreferences} />
        </div>
    );
};
