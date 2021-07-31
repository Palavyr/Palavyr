import React from "react";
import { IMessage, Link, CustomCompMessage, WidgetPreferences } from "@Palavyr-Types";
import { MessageWrapper } from "./MessageWrapper";

export const getComponentToRender = (message: IMessage | Link | CustomCompMessage, customPreferences: WidgetPreferences, showTimeStamp: boolean) => {
    const ComponentToRender = message.component;
    if (message.type === "component") {
        return (
            <MessageWrapper customPreferences={customPreferences}>
                <ComponentToRender {...message.props} />
            </MessageWrapper>
        );
    }
    return <ComponentToRender message={message} showTimeStamp={showTimeStamp} />;
};
