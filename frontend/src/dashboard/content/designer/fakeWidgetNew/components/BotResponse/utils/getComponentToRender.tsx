import React from "react";
import { IMessage, Link, CustomCompMessage } from "@Palavyr-Types";
import { MessageWrapper } from "./MessageWrapper";

export const getComponentToRender = (message: IMessage | Link | CustomCompMessage, showTimeStamp: boolean) => {
    const ComponentToRender = message.component;
    if (message.type === "component") {
        return (
            <MessageWrapper>
                <ComponentToRender />
            </MessageWrapper>
        );
    }
    return <ComponentToRender message={message} showTimeStamp={showTimeStamp} />;
};
